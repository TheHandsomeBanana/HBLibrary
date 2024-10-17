using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Account;
public interface IAccountStorage {
    public List<AccountInfo> LoadAccounts();
    public Task<AccountInfo[]> LoadAccountsAsync(CancellationToken cancellationToken = default);
    public bool AccountExists(string identifier);
    public Task<bool> AccountExistsAsync(string identifier, CancellationToken cancellationToken = default);
    public AccountInfo? GetAccount(string identifier);
    public Task<AccountInfo?> GetAccountAsync(string identifier, CancellationToken cancellationToken = default);
    public AccountInfo? GetLatestAccount(string application);
    public Task<AccountInfo?> GetLatestAccountAsync(string application, CancellationToken cancellationToken = default);
    public void AddOrUpdateAccount(AccountInfo account);
    public Task AddOrUpdateAccountAsync(AccountInfo account, CancellationToken cancellationToken = default);
    public void RemoveAccount(string identifier);
    public Task RemoveAccountAsync(string identifier, CancellationToken cancellationToken = default);
    public void RemoveApplicationFromAccount(string identifier, string application);
    public Task RemoveApplicationFromAccountAsync(string identifier, string application, CancellationToken cancellationToken = default);
    public Task<AccountInfo> GetUpdatedOrNewAsync(string identifier, string application, Func<AccountInfo> newFunc, CancellationToken cancellationToken = default);
}
