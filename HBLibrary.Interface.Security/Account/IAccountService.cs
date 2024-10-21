using HBLibrary.Interface.Security.Authentication;

namespace HBLibrary.Interface.Security.Account;
public interface IAccountService {
    public bool IsLoggedIn { get; }
    public IAccount? Account { get; }
    public IAccountStorage AccountStorage { get; }

    public Task RegisterAsync(IAuthCredentials credentials, string application, CancellationToken cancellationToken = default);
    public Task LoginAsync(IAuthCredentials credentials, string application, CancellationToken cancellationToken = default);
    public Task LogoutAsync(CancellationToken cancellationToken = default);
}
