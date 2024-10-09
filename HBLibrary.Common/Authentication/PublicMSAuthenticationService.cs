using HBLibrary.Common.Authentication.Microsoft;
using HBLibrary.Common.Exceptions;
using HBLibrary.Common.Security;
using HBLibrary.Common.Security.Keys;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Identity.Client;
using System.Net.Http;
using System.Security;



namespace HBLibrary.Common.Authentication;
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



    public async Task<MSAuthResult> AuthenticateAsync(MSAuthCredentials authCredentials, CancellationToken cancellationToken) {
        AuthenticationResult? result = null;
        string email = "";
        string displayName = "";
        string id = "";

        try {
            switch (authCredentials.Type) {
                case MSAuthCredentials.CredentialType.Cached:
                    IAccount? account = await app.GetAccountAsync(authCredentials.Identifier);
                    if (account is null) {
                        goto case MSAuthCredentials.CredentialType.Interactive;
                    }

                    AcquireTokenSilentParameterBuilder silentBuilder;
                    try {
                        silentBuilder = app.AcquireTokenSilent(authCredentials.Scopes, account);
                        authCredentials.SilentParameterBuilder?.Invoke(silentBuilder);
                        result = await silentBuilder.ExecuteAsync(cancellationToken);

                        email = authCredentials.Email!;
                        displayName = authCredentials.DisplayName!;
                        id = authCredentials.UserId!;
                    }
                    catch (MsalException) {
                        goto case MSAuthCredentials.CredentialType.Interactive;
                    }
                    break;

                case MSAuthCredentials.CredentialType.Silent:
                    try {

                        account = await app.GetAccountAsync(authCredentials.Identifier);
                        silentBuilder = app.AcquireTokenSilent(authCredentials.Scopes, account);
                        authCredentials.SilentParameterBuilder?.Invoke(silentBuilder);
                        result = await silentBuilder.ExecuteAsync(cancellationToken);

                        (id, email, displayName) = await GetUserDetailsAsync(result.AccessToken);
                    }
                    catch (MsalUiRequiredException) {
                        goto case MSAuthCredentials.CredentialType.Interactive;
                    }
                    break;

                case MSAuthCredentials.CredentialType.Interactive:
                    AcquireTokenInteractiveParameterBuilder interactiveBuilder = app.AcquireTokenInteractive(authCredentials.Scopes);
                    authCredentials.InteractiveParameterBuilder?.Invoke(interactiveBuilder);

                    result = await interactiveBuilder.ExecuteAsync(cancellationToken);

                    await RegisterStorageIdentity(result, cancellationToken);
                    (id, email, displayName) = await GetUserDetailsAsync(result.AccessToken);
                    break;

                case MSAuthCredentials.CredentialType.UsernamePassword:
                    AcquireTokenByUsernamePasswordParameterBuilder usernamePasswordBuilder =
                        app.AcquireTokenByUsernamePassword(authCredentials.Scopes, authCredentials.Username, SStringConverter.SecureStringToString(authCredentials.Password!));

                    authCredentials.UsernamePasswordParameterBuilder?.Invoke(usernamePasswordBuilder);
                    result = await usernamePasswordBuilder.ExecuteAsync(cancellationToken);

                    await RegisterStorageIdentity(result, cancellationToken);
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
            Email = email,
            DisplayName = displayName
        };
    }

    public async Task<AuthenticationResult> GetAccessTokenAsync(string identifier, IEnumerable<string> scopes) {
        IAccount account = await this.app.GetAccountAsync(identifier)
            ?? throw new AuthenticationException($"Account with identifier {identifier} not found.");

        try {
            return await this.app.AcquireTokenSilent(scopes, account).ExecuteAsync();
        }
        catch (MsalException ex) {
            throw AuthenticationException.AuthenticationFailed(ex);
        }
    }

    public async Task SignOutAsync(string identifier, CancellationToken cancellationToken = default) {
        IAccount? account = await this.app.GetAccountAsync(identifier);
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

    private async Task<MicrosoftIdentity> RegisterStorageIdentity(AuthenticationResult result, CancellationToken cancellationToken = default) {
        (string id, string email, string displayName) = await GetUserDetailsAsync(result.AccessToken);

        byte[] salt = GlobalEnvironment.Encoding.GetBytes($"{result!.Account.Username}.{email}");
        string temp = $"{result.TenantId}.{result.Account.HomeAccountId.Identifier}";
        SecureString password = KeyDerivation.DeriveNewSecureString(temp, salt);

        AccountKeyManager accountKeyManager = new AccountKeyManager();
        // Try to create keys, they might already exist -> swallow exception
        await accountKeyManager.CreateAccountKeysAsync(id, password, salt);


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
