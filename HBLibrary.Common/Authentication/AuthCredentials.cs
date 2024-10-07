using HBLibrary.Common.Authentication.Microsoft;
using HBLibrary.Common.Security;
using Microsoft.Identity.Client;
using System.Security;

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
    internal string? Identifier { get; init; }
    internal string? Username { get; init; }
    internal SecureString? Password { get; init; }
    internal string? Email { get; init; }
    internal string? DisplayName { get; init; }

    private MSAuthCredentials() { }

    internal enum CredentialType {
        Cached,
        Silent,
        Interactive,
        UsernamePassword
    }

    public static async Task<MSAuthCredentials?> CreateFromParameterStorageAsync(string username, Action<AcquireTokenSilentParameterBuilder>? builder = null) {
        MSParameterStorage storage = new MSParameterStorage();
        MicrosoftIdentity? identity = await storage.GetIdentityAsync(username);

        if (identity is null) {
            return null;
        }

        MSAuthCredentials credentials = new MSAuthCredentials {
            Scopes = identity.Scopes,
            Identifier = identity.Identifier,
            Email = identity.Email,
            DisplayName = identity.DisplayName,
            Type = CredentialType.Cached,
            SilentParameterBuilder = builder
        };

        return credentials;
    }

    public static MSAuthCredentials? CreateFromParameterStorage(string username,
        Action<AcquireTokenSilentParameterBuilder>? builder = null, Action<AcquireTokenInteractiveParameterBuilder>? fallbackBuilder = null) {

        MSParameterStorage storage = new MSParameterStorage();
        MicrosoftIdentity? identity = storage.GetIdentity(username);

        if (identity is null) {
            return null;
        }

        MSAuthCredentials credentials = new MSAuthCredentials {
            Scopes = identity.Scopes,
            Identifier = identity.Identifier,
            Email = identity.Email,
            DisplayName = identity.DisplayName,
            Type = CredentialType.Cached,
            SilentParameterBuilder = builder,
            InteractiveParameterBuilder = fallbackBuilder
        };

        return credentials;
    }

    public static MSAuthCredentials CreateSilent(IEnumerable<string> scopes, string identifier,
        Action<AcquireTokenSilentParameterBuilder>? builder = null, Action<AcquireTokenInteractiveParameterBuilder>? fallbackBuilder = null) {

        MSAuthCredentials credentials = new MSAuthCredentials {
            Scopes = scopes,
            Identifier = identifier,
            Type = CredentialType.Silent,
            SilentParameterBuilder = builder,
            InteractiveParameterBuilder = fallbackBuilder
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
