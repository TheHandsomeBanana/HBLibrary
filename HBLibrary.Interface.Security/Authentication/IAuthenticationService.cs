using Microsoft.Identity.Client;

namespace HBLibrary.Interface.Security.Authentication;
public interface IAuthenticationService<TAuthResult, TAuthCredentials> where TAuthResult : IAuthResult where TAuthCredentials : IAuthCredentials {
    public Task<TAuthResult> AuthenticateAsync(TAuthCredentials authCredentials, CancellationToken cancellationToken = default);
}

public interface ILocalAuthenticationService : IAuthenticationService<ILocalAuthResult, ILocalAuthCredentials> {
    public Task<bool> IsNewUserAsync(string username, CancellationToken cancellationToken = default);
    public Task<ILocalAuthResult> AuthenticateNewAsync(ILocalAuthCredentials authCredentials, CancellationToken cancellationToken = default);
    public Task DeleteLocalUser(string username, CancellationToken cancellationToken = default);
}

public interface IPublicMSAuthenticationService : IAuthenticationService<IMSAuthResult, IMSAuthCredentials> {
    public Task<AuthenticationResult> GetAccessTokenAsync(string identifier, IEnumerable<string> scopes);
    public Task SignOutAsync(string identifier, CancellationToken cancellationToken = default);
    public Task SignOutAsync(IAccount account, CancellationToken cancellationToken = default);
}
