using HBLibrary.Common.Security;
using Microsoft.Identity.Client;
using System.Runtime.Versioning;

namespace HBLibrary.Common.Authentication.Microsoft;
#if WINDOWS
#if NET5_0_OR_GREATER
[SupportedOSPlatform("windows")]
#endif
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
            byte[] protectedData = File.ReadAllBytes(tokenStoragePath);
            byte[] data = DPApi.Unprotect(protectedData);

            args.TokenCache.DeserializeMsalV3(data);
        }
    }

    private void AfterAccessNotification(TokenCacheNotificationArgs args) {
        if (args.HasStateChanged) {
            byte[] data = args.TokenCache.SerializeMsalV3();
            byte[] protectedData = DPApi.Protect(data);

            File.WriteAllBytes(tokenStoragePath, protectedData);
        }
    }
}
#endif