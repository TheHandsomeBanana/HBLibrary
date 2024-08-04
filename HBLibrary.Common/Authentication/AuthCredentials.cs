using HBLibrary.Common.Security;
using Microsoft.Identity.Client;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Authentication;
public interface IAuthCredentials { // Flag
}

public sealed class LocalAuthCredentials : IAuthCredentials {
    public string Username { get; set; }
    public SecureString Password { get; set; }

    public LocalAuthCredentials(string username, string password) {
        Username = username;
        Password = SStringConverter.StringToSecureString(password);
    }

    public LocalAuthCredentials(string username, SecureString password) {
        Username = username;
        Password = password;
    }
}

public sealed class MSAuthCredentials : IAuthCredentials {
    internal CredentialType Type { get; init; }
    public Action<AcquireTokenSilentParameterBuilder>? SilentParameterBuilder { get; init; }
    public Action<AcquireTokenInteractiveParameterBuilder>? InteractiveParameterBuilder { get; init; }
    public Action<AcquireTokenByUsernamePasswordParameterBuilder>? UsernamePasswordParameterBuilder { get; init; }
    public IEnumerable<string> Scopes { get; init; } = [];
    internal IAccount? Account { get; init; }
    internal string? Username { get; init; }
    internal SecureString? Password { get; init; }

    private MSAuthCredentials() { }

    internal enum CredentialType {
        Silent,
        Interactive,
        UsernamePassword
    }

    public static MSAuthCredentials CreateSilent(IEnumerable<string> scopes, IAccount account, Action<AcquireTokenSilentParameterBuilder>? builder = null) {

        MSAuthCredentials credentials = new MSAuthCredentials {
            Scopes = scopes,
            Account = account,
            Type = CredentialType.Silent,
            SilentParameterBuilder = builder
        };

        return credentials;
    }

    public static MSAuthCredentials CreateInteractive(IEnumerable<string> scopes, Action<AcquireTokenInteractiveParameterBuilder>? builder = null) {

        MSAuthCredentials credentials = new MSAuthCredentials {
            Type = CredentialType.Interactive,
            Scopes = scopes,
            InteractiveParameterBuilder = builder
        };

        return credentials;
    }

    public static MSAuthCredentials CreateByUsernamePassword(IEnumerable<string> scopes, string username, string password, Action<AcquireTokenByUsernamePasswordParameterBuilder> builder) {
        MSAuthCredentials credentials = new MSAuthCredentials {
            Type = CredentialType.UsernamePassword,
            Scopes = scopes,
            Username = username,
            Password = SStringConverter.StringToSecureString(password),
            UsernamePasswordParameterBuilder = builder
        };

        return credentials;
    }
}
