﻿using Microsoft.Identity.Client;

namespace HBLibrary.Common.Authentication;
public interface IAuthenticationService<TAuthResult, TAuthCredentials> where TAuthResult : AuthResult where TAuthCredentials : IAuthCredentials {
    public Task<TAuthResult> AuthenticateAsync(TAuthCredentials authCredentials, CancellationToken cancellationToken = default);
}

public interface ILocalAuthenticationService : IAuthenticationService<LocalAuthResult, LocalAuthCredentials> {
    public Task<bool> IsNewUserAsync(string username, CancellationToken cancellationToken = default);
    public Task<LocalAuthResult> AuthenticateNewAsync(LocalAuthCredentials authCredentials, CancellationToken cancellationToken = default);
    public Task DeleteLocalUser(string username, CancellationToken cancellationToken = default);
}

public interface IPublicMSAuthenticationService : IAuthenticationService<MSAuthResult, MSAuthCredentials> {
    public Task<AuthenticationResult> GetAccessTokenAsync(string identifier, IEnumerable<string> scopes);
    public Task SignOutAsync(string identifier, CancellationToken cancellationToken = default);
    public Task SignOutAsync(IAccount account, CancellationToken cancellationToken = default);
}
