using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Authentication.Microsoft;
public class MSTokenStorage {
    private readonly string tokenStoragePath;
    private readonly IPublicClientApplication app;

    private MSTokenStorage(string application, IPublicClientApplication app) {
        this.tokenStoragePath = Path.Combine(GlobalEnvironment.IdentityPath, application + ".tokencache");
        this.app = app;

        app.UserTokenCache.SetBeforeAccess(BeforeAccessNotification);
        app.UserTokenCache.SetAfterAccess(AfterAccessNotification);
    }

    public static void Create(string application, IPublicClientApplication app) {
        new MSTokenStorage(application, app);
    }

    private void BeforeAccessNotification(TokenCacheNotificationArgs args) {
        if (File.Exists(tokenStoragePath)) {
            args.TokenCache.DeserializeMsalV3(File.ReadAllBytes(tokenStoragePath));
        }
    }

    private void AfterAccessNotification(TokenCacheNotificationArgs args) {
        if (args.HasStateChanged) {
            File.WriteAllBytes(tokenStoragePath, args.TokenCache.SerializeMsalV3());
        }
    }
}
