using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Authentication;
public class PublicMSAuthenticationService {
    private readonly IPublicClientApplication app;
    private readonly string[] scopes = ["User.Read"];

    public PublicMSAuthenticationService(string clientId) {
        app = PublicClientApplicationBuilder.Create(clientId)
            .WithAuthority(AzureCloudInstance.AzurePublic, AadAuthorityAudience.AzureAdAndPersonalMicrosoftAccount)
            .WithRedirectUri("https://login.microsoftonline.com/common/oauth2/nativeclient")
            .Build();
    }


    public Task<AuthenticationResult> AuthenticateInteractiveAsync() {
        return app.AcquireTokenInteractive(scopes).ExecuteAsync();
    }
}
