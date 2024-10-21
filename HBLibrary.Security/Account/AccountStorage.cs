using HBLibrary.Core;
using HBLibrary.Interface.Security.Account;
using System.Linq;
using System.Text.Json;
using System.Threading;

namespace HBLibrary.Security.Account;
public class AccountStorage : IAccountStorage {
    private readonly string accountStoragePath;

    public AccountStorage() {
        accountStoragePath = Path.Combine(GlobalEnvironment.IdentityPath, "accountstorage");
        if (!File.Exists(accountStoragePath)) {
            File.Create(accountStoragePath).Dispose();
        }
    }

    public IAccountInfo[] LoadAccounts() {
        string base64Json = File.ReadAllText(accountStoragePath);
        string json = GlobalEnvironment.Encoding.GetString(Convert.FromBase64String(base64Json));
        if (string.IsNullOrWhiteSpace(json)) {
            return [];
        }

        return JsonSerializer.Deserialize<AccountInfo[]>(json) ?? [];
    }
    
    private List<AccountInfo> LoadAccountsInternal() {
        string base64Json = File.ReadAllText(accountStoragePath);
        string json = GlobalEnvironment.Encoding.GetString(Convert.FromBase64String(base64Json));
        if (string.IsNullOrWhiteSpace(json)) {
            return [];
        }

        return JsonSerializer.Deserialize<List<AccountInfo>>(json) ?? [];
    }

    public async Task<IAccountInfo[]> LoadAccountsAsync(CancellationToken cancellationToken = default) {
        return [.. (await LoadAccountsAsyncInternal(cancellationToken))];
    }

    private async Task<List<AccountInfo>> LoadAccountsAsyncInternal(CancellationToken cancellationToken = default) {
        string base64Json = await UnifiedFile.ReadAllTextAsync(accountStoragePath, cancellationToken);
        string json = GlobalEnvironment.Encoding.GetString(Convert.FromBase64String(base64Json));

        if (string.IsNullOrWhiteSpace(json)) {
            return [];
        }

        return JsonSerializer.Deserialize<List<AccountInfo>>(json) ?? [];
    }

    public bool AccountExists(string identifier) {
        List<AccountInfo> accounts = LoadAccountsInternal();
        return accounts.Any(e => e.AccountId == identifier);
    }

    public async Task<bool> AccountExistsAsync(string identifier, CancellationToken cancellationToken = default) {
        List<AccountInfo> accounts = await LoadAccountsAsyncInternal(cancellationToken);
        return accounts.Any(e => e.AccountId == identifier);
    }

    public IAccountInfo? GetAccount(string identifier) {
        List<AccountInfo> accounts = LoadAccountsInternal();
        return accounts.FirstOrDefault(e => e.AccountId == identifier);
    }

    public async Task<IAccountInfo?> GetAccountAsync(string identifier, CancellationToken cancellationToken = default) {
        List<AccountInfo> accounts = await LoadAccountsAsyncInternal(cancellationToken);
        return accounts.FirstOrDefault(e => e.AccountId == identifier);
    }

    public IAccountInfo? GetLatestAccount(string application) {
        List<AccountInfo> accounts = LoadAccountsInternal();

        return accounts.OrderByDescending(e =>
           e.Applications
               .Where(f => f.Application == application)
               .Select(f => f.LastLogin)
               .DefaultIfEmpty(DateTime.MinValue)
               .Max()
        ).FirstOrDefault();
    }

    public async Task<IAccountInfo?> GetLatestAccountAsync(string application, CancellationToken cancellationToken = default) {
        List<AccountInfo> accounts = await LoadAccountsAsyncInternal(cancellationToken);

        return accounts.OrderByDescending(e =>
           e.Applications
               .Where(f => f.Application == application)
               .Select(f => f.LastLogin)
               .DefaultIfEmpty(DateTime.MinValue)
               .Max()
        ).FirstOrDefault();
    }

    public void AddOrUpdateAccount(IAccountInfo accountInfo) {
        List<AccountInfo> accounts = LoadAccountsInternal();

        AccountInfo? existingAccount = accounts.FirstOrDefault(e => e == (AccountInfo)accountInfo);

        if (existingAccount is not null) {
            existingAccount.ModifiedOn = DateTime.UtcNow;
            existingAccount.AccountType = accountInfo.AccountType;
        }
        else {
            accounts.Add((AccountInfo)accountInfo);
        }

        SaveAllAccounts(accounts);
    }

    public async Task AddOrUpdateAccountAsync(IAccountInfo accountInfo, CancellationToken cancellationToken = default) {
        List<AccountInfo> accounts = await LoadAccountsAsyncInternal(cancellationToken);

        if (cancellationToken.IsCancellationRequested) {
            return;
        }

        AccountInfo? existingAccount = accounts.FirstOrDefault(e => e == (AccountInfo)accountInfo);

        if (existingAccount is not null) {
            accounts.Remove(existingAccount);
        }

        accounts.Add((AccountInfo)accountInfo);

        await SaveAllAccountsAsync(accounts);
    }

    public void RemoveAccount(string identifier) {
        List<AccountInfo> accounts = LoadAccountsInternal();
        AccountInfo? foundAccount = accounts.FirstOrDefault(e => e.AccountId == identifier);
        if (foundAccount is null) {
            return;
        }

        accounts.Remove(foundAccount);
        SaveAllAccounts(accounts);
    }

    public async Task RemoveAccountAsync(string identifier, CancellationToken cancellationToken = default) {
        List<AccountInfo> accounts = await LoadAccountsAsyncInternal(cancellationToken);
        AccountInfo? foundAccount = accounts.FirstOrDefault(e => e.AccountId == identifier);
        if (foundAccount is null) {
            return;
        }

        accounts.Remove(foundAccount);
        await SaveAllAccountsAsync(accounts, cancellationToken);
    }

    public void RemoveApplicationFromAccount(string identifier, string application) {
        IAccountInfo? foundAccount = GetAccount(identifier);
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
        IAccountInfo? foundAccount = await GetAccountAsync(identifier);
        if (foundAccount is null) {
            return;
        }

        AccountApplicationInfo? foundApplicationInfo = foundAccount.Applications.FirstOrDefault(e => e.Application == application);
        if (foundApplicationInfo is null) {
            return;
        }

        foundAccount.Applications.Remove(foundApplicationInfo);
    }


    public async Task<IAccountInfo> GetUpdatedOrNewAsync(string identifier, string application, Func<IAccountInfo> newFunc, CancellationToken cancellationToken = default) {
        IAccountInfo? accountInfo = await GetAccountAsync(identifier, cancellationToken);

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

        return UnifiedFile.WriteAllTextAsync(accountStoragePath, base64Json, cancellationToken);
    }
}
