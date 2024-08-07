using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Authentication;
public interface IAuthenticationService<TAuthResult, TAuthCredentials> where TAuthResult : IAuthResult where TAuthCredentials : IAuthCredentials {
    public Task<TAuthResult> AuthenticateAsync(TAuthCredentials authCredentials, CancellationToken cancellationToken = default);
}

public interface ILocalAuthenticationService : IAuthenticationService<LocalAuthResult, LocalAuthCredentials> {

}

public interface IPublicMSAuthenticationService : IAuthenticationService<MSAuthResult, MSAuthCredentials> {
    public Task<AuthenticationResult> GetAccessTokenAsync(string identifier, IEnumerable<string> scopes);
    public Task SignOutAsync(string identifier, CancellationToken cancellationToken = default);
    public Task SignOutAsync(IAccount account, CancellationToken cancellationToken = default);
}
