using System.Linq;
using System.Text.Json;
using System.Threading;

namespace HBLibrary.Common.Account;
public class AccountStorage : IAccountStorage {
    private readonly string accountStoragePath;

    public AccountStorage() {
        accountStoragePath = Path.Combine(GlobalEnvironment.IdentityPath, "accountstorage");
        if (!File.Exists(accountStoragePath)) {
            File.Create(accountStoragePath).Dispose();
        }
    }

    public List<AccountInfo> LoadAccounts() {
        string base64Json = File.ReadAllText(accountStoragePath);
        string json = GlobalEnvironment.Encoding.GetString(Convert.FromBase64String(base64Json));
        if (string.IsNullOrWhiteSpace(json)) {
            return [];
        }

        return JsonSerializer.Deserialize<List<AccountInfo>>(json) ?? [];
    }

    public async Task<List<AccountInfo>> LoadAccountsAsync(CancellationToken cancellationToken = default) {
        string base64Json;
#if NET5_0_OR_GREATER
        base64Json = await File.ReadAllTextAsync(accountStoragePath, cancellationToken);
#elif NET472_OR_GREATER
        using (FileStream fs = new FileStream(accountStoragePath, FileMode.Open, FileAccess.Read)) {
            using (StreamReader sr = new StreamReader(fs)) {
                base64Json = await sr.ReadToEndAsync();
            }
        }
#endif

        string json = GlobalEnvironment.Encoding.GetString(Convert.FromBase64String(base64Json));

        if (string.IsNullOrWhiteSpace(json)) {
            return [];
        }

        return JsonSerializer.Deserialize<List<AccountInfo>>(json) ?? [];
    }

    public bool AccountExists(string identifier) {
        List<AccountInfo> accounts = LoadAccounts();
        return accounts.Any(e => e.AccountId == identifier);
    }

    public async Task<bool> AccountExistsAsync(string identifier, CancellationToken cancellationToken = default) {
        List<AccountInfo> accounts = await LoadAccountsAsync(cancellationToken);
        return accounts.Any(e => e.AccountId == identifier);
    }

    public AccountInfo? GetAccount(string identifier) {
        List<AccountInfo> accounts = LoadAccounts();
        return accounts.FirstOrDefault(e => e.AccountId == identifier);
    }

    public async Task<AccountInfo?> GetAccountAsync(string identifier, CancellationToken cancellationToken = default) {
        List<AccountInfo> accounts = await LoadAccountsAsync(cancellationToken);
        return accounts.FirstOrDefault(e => e.AccountId == identifier);
    }

    public AccountInfo? GetLatestAccount(string application) {
        List<AccountInfo> accounts = LoadAccounts();

        return accounts.OrderByDescending(e => {

            return e.Applications.Where(f => f.Application == application)
             .Select(e => e.LastLogin);

        }).FirstOrDefault();
    }

    public async Task<AccountInfo?> GetLatestAccountAsync(string application, CancellationToken cancellationToken = default) {
        List<AccountInfo> accounts = await LoadAccountsAsync(cancellationToken);

        return accounts.OrderByDescending(e => {

            return e.Applications.Where(f => f.Application == application)
             .Select(e => e.LastLogin);

        }).FirstOrDefault();
    }

    public void AddOrUpdateAccount(AccountInfo accountInfo) {
        List<AccountInfo> accounts = LoadAccounts();

        AccountInfo? existingAccount = accounts.FirstOrDefault(e => e == accountInfo);

        if (existingAccount is not null) {
            existingAccount.ModifiedOn = DateTime.UtcNow;
            existingAccount.AccountType = accountInfo.AccountType;
        }
        else {
            accounts.Add(accountInfo);
        }

        SaveAllAccounts(accounts);
    }

    public async Task AddOrUpdateAccountAsync(AccountInfo accountInfo, CancellationToken cancellationToken = default) {
        List<AccountInfo> accounts = await LoadAccountsAsync(cancellationToken);

        if (cancellationToken.IsCancellationRequested) {
            return;
        }

        AccountInfo? existingAccount = accounts.FirstOrDefault(e => e == accountInfo);

        if (existingAccount is not null) {
            existingAccount.ModifiedOn = DateTime.UtcNow;
        }
        else {
            accounts.Add(accountInfo);
        }

        await SaveAllAccountsAsync(accounts);
    }

    public void RemoveAccount(string identifier) {
        List<AccountInfo> accounts = LoadAccounts();
        AccountInfo? foundAccount = accounts.FirstOrDefault(e => e.AccountId == identifier);
        if(foundAccount is null) {
            return;
        }

        accounts.Remove(foundAccount);
        SaveAllAccounts(accounts);
    }

    public async Task RemoveAccountAsync(string identifier, CancellationToken cancellationToken = default) {
        List<AccountInfo> accounts = await LoadAccountsAsync(cancellationToken);
        AccountInfo? foundAccount = accounts.FirstOrDefault(e => e.AccountId == identifier);
        if (foundAccount is null) {
            return;
        }

        accounts.Remove(foundAccount);
        await SaveAllAccountsAsync(accounts, cancellationToken);
    }

    public void RemoveApplicationFromAccount(string identifier, string application) {
        AccountInfo? foundAccount = GetAccount(identifier);
        if (foundAccount is null) {
            return;
        }

        AccountApplicationInfo? foundApplicationInfo = foundAccount.Applications.FirstOrDefault(e => e.Application == application);
        if (foundApplicationInfo is null) {
            return;
        }

        foundAccount.Applications.Remove(foundApplicationInfo);
    }

    public async Task RemoveApplicationFromAccountAsync(string identifier, string application, CancellationToken cancellationToken = default) {
        AccountInfo? foundAccount = await GetAccountAsync(identifier);
        if (foundAccount is null) {
            return;
        }

        AccountApplicationInfo? foundApplicationInfo = foundAccount.Applications.FirstOrDefault(e => e.Application == application);
        if (foundApplicationInfo is null) {
            return;
        }

        foundAccount.Applications.Remove(foundApplicationInfo);
    }


    public async Task<AccountInfo> GetUpdatedOrNewAsync(string identifier, string application, Func<AccountInfo> newFunc, CancellationToken cancellationToken = default) {
        AccountInfo? accountInfo = await GetAccountAsync(identifier, cancellationToken);

        if (accountInfo is not null) {
            bool applicationFound = false;
            foreach (AccountApplicationInfo applicationInfo in accountInfo.Applications) {
                if (applicationInfo.Application == application) {
                    applicationInfo.LastLogin = DateTime.UtcNow;
                    applicationFound = true;
                    break;
                }
            }

            if (!applicationFound) {
                accountInfo.Applications.Add(new AccountApplicationInfo {
                    Application = application,
                    InitialLogin = DateTime.UtcNow,
                    LastLogin = DateTime.UtcNow,
                });
            }
        }
        else {
            accountInfo = newFunc();
        }

        return accountInfo;
    }

    private void SaveAllAccounts(List<AccountInfo> accounts) {
        string json = JsonSerializer.Serialize(accounts);
        string base64Json = Convert.ToBase64String(GlobalEnvironment.Encoding.GetBytes(json));

        File.WriteAllText(accountStoragePath, base64Json);
    }

    private Task SaveAllAccountsAsync(List<AccountInfo> accounts, CancellationToken cancellationToken = default) {
        string json = JsonSerializer.Serialize(accounts);
        string base64Json = Convert.ToBase64String(GlobalEnvironment.Encoding.GetBytes(json));

#if NET5_0_OR_GREATER
        return File.WriteAllTextAsync(accountStoragePath, base64Json, cancellationToken);
#elif NET472_OR_GREATER
        using (FileStream fs = new FileStream(accountStoragePath, FileMode.Open, FileAccess.Read)) {
            using (StreamWriter sw = new StreamWriter(fs)) {
                return sw.WriteAsync(base64Json);
            }
        }
#endif
    }
}
