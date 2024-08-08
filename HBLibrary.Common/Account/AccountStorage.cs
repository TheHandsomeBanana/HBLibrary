using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace HBLibrary.Common.Account;
public class AccountStorage {
    private readonly string accountStoragePath;

    public AccountStorage() {
        accountStoragePath = Path.Combine(GlobalEnvironment.IdentityPath, "applications.accounts");
        if (!File.Exists(accountStoragePath)) {
            File.Create(accountStoragePath).Dispose();
        }
    }

    public AccountInfo? GetAccount(string application) {
        List<AccountInfo> accounts = LoadAllAccounts();
        return accounts.FirstOrDefault(e => e.Application == application);
    }

    public void AddOrUpdateAccount(AccountInfo accountInfo) {
        List<AccountInfo> accounts = LoadAllAccounts();

        AccountInfo? existingAccount = accounts.FirstOrDefault(e => e.Application == accountInfo.Application);

        if (existingAccount is not null) {
            existingAccount.ModifiedOn = DateTime.UtcNow;
            existingAccount.AccountType = accountInfo.AccountType;
        }
        else {
            accountInfo.CreatedOn = DateTime.UtcNow;
            accountInfo.ModifiedOn = DateTime.UtcNow;
            accounts.Add(accountInfo);
        }

        SaveAllAccounts(accounts);
    }

    public async Task<AccountInfo?> GetAccountAsync(string application, CancellationToken cancellationToken = default) {
        List<AccountInfo> accounts = await LoadAllAccountsAsync(cancellationToken);

        return accounts.FirstOrDefault(e => e.Application == application);
    }

    public async Task AddOrUpdateAccountAsync(AccountInfo accountInfo, CancellationToken cancellationToken = default) {
        List<AccountInfo> accounts = await LoadAllAccountsAsync(cancellationToken);

        AccountInfo? existingAccount = accounts.FirstOrDefault(e => e.Application == accountInfo.Application);

        if (existingAccount is not null) {
            existingAccount.ModifiedOn = DateTime.UtcNow;
            existingAccount.AccountType = accountInfo.AccountType;
            existingAccount.Username = accountInfo.Username;
        }
        else {
            accountInfo.CreatedOn = DateTime.UtcNow;
            accountInfo.ModifiedOn = DateTime.UtcNow;
            accounts.Add(accountInfo);
        }

        await SaveAllAccountsAsync(accounts);
    }

    public List<AccountInfo> LoadAllAccounts() {
        string base64Json = File.ReadAllText(accountStoragePath);
        string json = GlobalEnvironment.Encoding.GetString(Convert.FromBase64String(base64Json));
        if (string.IsNullOrWhiteSpace(json)) {
            return [];
        }

        return JsonSerializer.Deserialize<List<AccountInfo>>(json) ?? [];
    }

    public async Task<List<AccountInfo>> LoadAllAccountsAsync(CancellationToken cancellationToken = default) {
        string base64Json;
#if NET5_0_OR_GREATER
        base64Json = await File.ReadAllTextAsync(accountStoragePath, cancellationToken);
#elif NET472_OR_GREATER
        using(FileStream fs = new FileStream(accountStoragePath, FileMode.Open, FileAccess.Read)) {
            using(StreamReader sr = new StreamReader(fs)) {           
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
