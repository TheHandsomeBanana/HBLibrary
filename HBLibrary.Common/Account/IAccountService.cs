using HBLibrary.Common.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Account;
public interface IAccountService {
    public bool IsLoggedIn { get; }
    public Account? Account { get; }
    public Task LoginAsync(IAuthCredentials credentials, string application, CancellationToken cancellationToken = default);
    public Task LogoutAsync(CancellationToken cancellationToken = default);

    public Task<AccountInfo?> GetLastAccountAsync(string application, CancellationToken cancellationToken = default);
    public Task SaveAccountAsync(AccountInfo accountInfo);

    public AccountInfo? GetLastAccount(string application);
    public void SaveAccount(AccountInfo accountInfo);

}
