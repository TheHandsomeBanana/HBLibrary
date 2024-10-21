using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Security.Authentication;
public interface IAuthCredentials {
}

public interface ILocalAuthCredentials : IAuthCredentials {
    public string Username { get; set; }
    public SecureString Password { get; set; }
}

public interface IMSAuthCredentials : IAuthCredentials {
    public Action<AcquireTokenSilentParameterBuilder>? SilentParameterBuilder { get; init; }
    public Action<AcquireTokenInteractiveParameterBuilder>? InteractiveParameterBuilder { get; init; }
    public Action<AcquireTokenByUsernamePasswordParameterBuilder>? UsernamePasswordParameterBuilder { get; init; }
    public IEnumerable<string> Scopes { get; init; }
    public CredentialType Type { get; init; }
    public string? Identifier { get; init; }
    public string? UserId { get; init; }
    public string? Username { get; init; }
    public SecureString? Password { get; init; }
    public string? Email { get; init; }
    public string? DisplayName { get; init; }

    public enum CredentialType {
        Cached,
        Silent,
        Interactive,
        UsernamePassword
    }
}