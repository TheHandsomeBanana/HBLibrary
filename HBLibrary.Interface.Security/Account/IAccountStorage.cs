using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Security.Account;
public interface IAccountStorage {
    public IAccountInfo[] LoadAccounts();
    public Task<IAccountInfo[]> LoadAccountsAsync(CancellationToken cancellationToken = default);
    public bool AccountExists(string identifier);
    public Task<bool> AccountExistsAsync(string identifier, CancellationToken cancellationToken = default);
    public IAccountInfo? GetAccount(string identifier);
    public Task<IAccountInfo?> GetAccountAsync(string identifier, CancellationToken cancellationToken = default);
    public IAccountInfo? GetLatestAccount(string application);
    public Task<IAccountInfo?> GetLatestAccountAsync(string application, CancellationToken cancellationToken = default);
    public void AddOrUpdateAccount(IAccountInfo account);
    public Task AddOrUpdateAccountAsync(IAccountInfo account, CancellationToken cancellationToken = default);
    public void RemoveAccount(string identifier);
    public Task RemoveAccountAsync(string identifier, CancellationToken cancellationToken = default);
    public void RemoveApplicationFromAccount(string identifier, string application);
    public Task RemoveApplicationFromAccountAsync(string identifier, string application, CancellationToken cancellationToken = default);
    public Task<IAccountInfo> GetUpdatedOrNewAsync(string identifier, string application, Func<IAccountInfo> newFunc, CancellationToken cancellationToken = default);
}
