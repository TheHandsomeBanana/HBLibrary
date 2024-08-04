using HBLibrary.Common.Account;
using HBLibrary.Common.Security;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Authentication;
public sealed class PublicMSAuthenticationService : IAuthenticationService<MSAuthResult, MSAuthCredentials> {
    private readonly IPublicClientApplication app;
    private readonly IAccountService? accountService;

    public PublicMSAuthenticationService(string clientId, IAccountService? accountService = null) {
        app = PublicClientApplicationBuilder.Create(clientId)
            .WithAuthority(AzureCloudInstance.AzurePublic, AadAuthorityAudience.AzureAdAndPersonalMicrosoftAccount)
            .WithRedirectUri("https://login.microsoftonline.com/common/oauth2/nativeclient")
            .Build();

        this.accountService = accountService;
    }

    public PublicMSAuthenticationService(IPublicClientApplication clientApp, IAccountService? accountService = null) {
        this.app = clientApp;
        this.accountService = accountService;
    }

    public async Task<MSAuthResult> Authenticate(MSAuthCredentials authCredentials) {
        AuthenticationResult result;

        switch (authCredentials.Type) {
            case MSAuthCredentials.CredentialType.Silent:
                AcquireTokenSilentParameterBuilder silentBuilder = app.AcquireTokenSilent(authCredentials.Scopes, authCredentials.Account);
                authCredentials.SilentParameterBuilder?.Invoke(silentBuilder);
                result = await silentBuilder.ExecuteAsync();
                break;

            case MSAuthCredentials.CredentialType.Interactive:
                AcquireTokenInteractiveParameterBuilder interactiveBuilder = app.AcquireTokenInteractive(authCredentials.Scopes);
                authCredentials.InteractiveParameterBuilder?.Invoke(interactiveBuilder);

                result = await interactiveBuilder.ExecuteAsync();
                break;

            case MSAuthCredentials.CredentialType.UsernamePassword:
                AcquireTokenByUsernamePasswordParameterBuilder usernamePasswordBuilder =
                    app.AcquireTokenByUsernamePassword(authCredentials.Scopes, authCredentials.Username, SStringConverter.SecureStringToString(authCredentials.Password));

                authCredentials.UsernamePasswordParameterBuilder?.Invoke(usernamePasswordBuilder);
                result = await usernamePasswordBuilder.ExecuteAsync();
                break;
            default:
                throw new NotSupportedException(authCredentials.Type.ToString());
        }

        accountService?.SetCurrentAccount(new MicrosoftAccountDetails {
            Token = result.AccessToken,
            Account = result.Account,
            TenantId = result.TenantId
        });


        return new MSAuthResult {
            Result = result
        };
    }
}
