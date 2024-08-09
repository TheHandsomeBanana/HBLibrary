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

    public Task RegisterAsync(IAuthCredentials credentials, string application, CancellationToken cancellationToken = default);
    public Task LoginAsync(IAuthCredentials credentials, string application, CancellationToken cancellationToken = default);
    public Task LogoutAsync(CancellationToken cancellationToken = default);

    public Task<ApplicationAccountInfo?> GetLastAccountAsync(string application, CancellationToken cancellationToken = default);
    public Task SaveAccountAsync(ApplicationAccountInfo accountInfo);

    public ApplicationAccountInfo? GetLastAccount(string application);
    public void SaveAccount(ApplicationAccountInfo accountInfo);

}
