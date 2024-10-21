using HBLibrary.Interface.Security.Authentication;
using HBLibrary.Security;
using HBLibrary.Security.Authentication.Microsoft;
using Microsoft.Identity.Client;
using System.Security;
using static HBLibrary.Interface.Security.Authentication.IMSAuthCredentials;

namespace HBLibrary.Security.Authentication;

public sealed class LocalAuthCredentials : ILocalAuthCredentials {
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

public sealed class MSAuthCredentials : IMSAuthCredentials {
    public CredentialType Type { get; init; }
    public Action<AcquireTokenSilentParameterBuilder>? SilentParameterBuilder { get; init; }
    public Action<AcquireTokenInteractiveParameterBuilder>? InteractiveParameterBuilder { get; init; }
    public Action<AcquireTokenByUsernamePasswordParameterBuilder>? UsernamePasswordParameterBuilder { get; init; }
    public IEnumerable<string> Scopes { get; init; } = [];
    public string? Identifier { get; init; }
    public string? UserId { get; init; }
    public string? Username { get; init; }
    public SecureString? Password { get; init; }
    public string? Email { get; init; }
    public string? DisplayName { get; init; }

    private MSAuthCredentials() { }


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
            UserId = identity.UserId,
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
            UserId = identity.UserId,
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
