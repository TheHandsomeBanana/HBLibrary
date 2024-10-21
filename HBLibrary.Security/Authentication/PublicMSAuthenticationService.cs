using HBLibrary.Core;
using HBLibrary.DataStructures;
using HBLibrary.Interface.Security.Authentication;
using HBLibrary.Interface.Security.Keys;
using HBLibrary.Security.Authentication.Microsoft;
using HBLibrary.Security.Exceptions;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Identity.Client;
using System.Net.Http;
using System.Security;



namespace HBLibrary.Security.Authentication;
public sealed class PublicMSAuthenticationService : IPublicMSAuthenticationService {
    private readonly IPublicClientApplication app;
    private readonly MSParameterStorage parameterStorage;

    public PublicMSAuthenticationService(AzureAdOptions azureAdOptions) {
        app = PublicClientApplicationBuilder.Create(azureAdOptions.ClientId)
            .WithAuthority(AzureCloudInstance.AzurePublic, AadAuthorityAudience.AzureAdAndPersonalMicrosoftAccount)
            .WithRedirectUri(azureAdOptions.RedirectUri)
            .Build();

        parameterStorage = new MSParameterStorage();
    }

    public PublicMSAuthenticationService(IPublicClientApplication app) {
        this.app = app;
        parameterStorage = new MSParameterStorage();
    }



    public async Task<IMSAuthResult> AuthenticateAsync(IMSAuthCredentials authCredentials, CancellationToken cancellationToken) {
        AuthenticationResult? result = null;
        string email = "";
        string displayName = "";
        string id = "";

        byte[] salt;
        string supportKeyInput;
        SecureString supportKey;

        try {
            switch (authCredentials.Type) {
                case IMSAuthCredentials.CredentialType.Cached:
                    IAccount? account = await app.GetAccountAsync(authCredentials.Identifier);
                    if (account is null) {
                        goto case IMSAuthCredentials.CredentialType.Interactive;
                    }

                    AcquireTokenSilentParameterBuilder silentBuilder;
                    try {
                        silentBuilder = app.AcquireTokenSilent(authCredentials.Scopes, account);
                        authCredentials.SilentParameterBuilder?.Invoke(silentBuilder);
                        result = await silentBuilder.ExecuteAsync(cancellationToken);

                        salt = GlobalEnvironment.Encoding.GetBytes($"{result!.Account.Username}.{email}");
                        supportKeyInput = $"{result.TenantId}.{result.Account.HomeAccountId.Identifier}";
                        supportKey = KeyDerivation.DeriveNewSecureString(supportKeyInput, salt);

                        email = authCredentials.Email!;
                        displayName = authCredentials.DisplayName!;
                        id = authCredentials.UserId!;
                    }
                    catch (MsalException) {
                        goto case IMSAuthCredentials.CredentialType.Interactive;
                    }
                    break;

                case IMSAuthCredentials.CredentialType.Silent:
                    try {

                        account = await app.GetAccountAsync(authCredentials.Identifier);
                        silentBuilder = app.AcquireTokenSilent(authCredentials.Scopes, account);
                        authCredentials.SilentParameterBuilder?.Invoke(silentBuilder);
                        result = await silentBuilder.ExecuteAsync(cancellationToken);

                        salt = GlobalEnvironment.Encoding.GetBytes($"{result!.Account.Username}.{email}");
                        supportKeyInput = $"{result.TenantId}.{result.Account.HomeAccountId.Identifier}";
                        supportKey = KeyDerivation.DeriveNewSecureString(supportKeyInput, salt);

                        (id, email, displayName) = await GetUserDetailsAsync(result.AccessToken);
                    }
                    catch (MsalUiRequiredException) {
                        goto case IMSAuthCredentials.CredentialType.Interactive;
                    }
                    break;

                case IMSAuthCredentials.CredentialType.Interactive:
                    AcquireTokenInteractiveParameterBuilder interactiveBuilder = app.AcquireTokenInteractive(authCredentials.Scopes);
                    authCredentials.InteractiveParameterBuilder?.Invoke(interactiveBuilder);

                    result = await interactiveBuilder.ExecuteAsync(cancellationToken);

                    salt = GlobalEnvironment.Encoding.GetBytes($"{result!.Account.Username}.{email}");
                    supportKeyInput = $"{result.TenantId}.{result.Account.HomeAccountId.Identifier}";
                    supportKey = KeyDerivation.DeriveNewSecureString(supportKeyInput, salt);

                    await RegisterStorageIdentity(result, supportKey, salt, cancellationToken);
                    (id, email, displayName) = await GetUserDetailsAsync(result.AccessToken);
                    break;

                case IMSAuthCredentials.CredentialType.UsernamePassword:
                    AcquireTokenByUsernamePasswordParameterBuilder usernamePasswordBuilder =
                        app.AcquireTokenByUsernamePassword(authCredentials.Scopes, authCredentials.Username, SStringConverter.SecureStringToString(authCredentials.Password!));

                    authCredentials.UsernamePasswordParameterBuilder?.Invoke(usernamePasswordBuilder);
                    result = await usernamePasswordBuilder.ExecuteAsync(cancellationToken);

                    salt = GlobalEnvironment.Encoding.GetBytes($"{result!.Account.Username}.{email}");
                    supportKeyInput = $"{result.TenantId}.{result.Account.HomeAccountId.Identifier}";
                    supportKey = KeyDerivation.DeriveNewSecureString(supportKeyInput, salt);

                    await RegisterStorageIdentity(result, supportKey, salt, cancellationToken);
                    (id, email, displayName) = await GetUserDetailsAsync(result.AccessToken);
                    break;
                default:
                    throw new NotSupportedException(authCredentials.Type.ToString());
            }
        }
        catch (MsalException ex) {
            throw AuthenticationException.AuthenticationFailed(ex);
        }

        AccountKeyManager accountKeyManager = new AccountKeyManager();
        Result<RsaKey> publicKeyResult = await accountKeyManager.GetPublicKeyAsync(id);

        RsaKey publicKey = publicKeyResult.GetValueOrThrow();

        return new MSAuthResult {
            PublicKey = publicKey,
            Result = result,
            UserId = id,
            Email = email,
            DisplayName = displayName,
            Salt = GlobalEnvironment.Encoding.GetString(salt),
            SupportKey = supportKey
        };
    }

    public async Task<AuthenticationResult> GetAccessTokenAsync(string identifier, IEnumerable<string> scopes) {
        IAccount account = await app.GetAccountAsync(identifier)
            ?? throw new AuthenticationException($"Account with identifier {identifier} not found.");

        try {
            return await app.AcquireTokenSilent(scopes, account).ExecuteAsync();
        }
        catch (MsalException ex) {
            throw AuthenticationException.AuthenticationFailed(ex);
        }
    }

    public async Task SignOutAsync(string identifier, CancellationToken cancellationToken = default) {
        IAccount? account = await app.GetAccountAsync(identifier);
        if (account is not null) {
            await SignOutAsync(account, cancellationToken);
        }
    }

    public async Task SignOutAsync(IAccount account, CancellationToken cancellationToken = default) {
        await app.RemoveAsync(account);
        await parameterStorage.UnregisterIdentityAsync(account.Username, cancellationToken);
    }

    private async Task<(string, string, string)> GetUserDetailsAsync(string accessToken) {
        using (HttpClient httpClient = new HttpClient()) {
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            GraphServiceClient graphServiceClient = new GraphServiceClient(httpClient);

            User? me = await graphServiceClient.Me.GetAsync();
            string email = me?.Mail ?? me?.UserPrincipalName ?? string.Empty;
            string displayName = me?.DisplayName ?? string.Empty;
            string id = me?.Id ?? string.Empty;

            return (id, email, displayName);
        }
    }

    private async Task<MicrosoftIdentity> RegisterStorageIdentity(AuthenticationResult result, SecureString supportKey, byte[] salt, CancellationToken cancellationToken = default) {
        (string id, string email, string displayName) = await GetUserDetailsAsync(result.AccessToken);

        AccountKeyManager accountKeyManager = new AccountKeyManager();
        if (!accountKeyManager.KeyPairExists(id)) {
            await accountKeyManager.CreateAccountKeysAsync(id, supportKey, salt);
        }


        return await parameterStorage.RegisterIdentityAsync(result.Account.Username,
                        result.Account.HomeAccountId.Identifier,
                        id,
                        email,
                        displayName,
                        result.Scopes.ToArray(),
                        result.TenantId,
                        cancellationToken);
    }
}
