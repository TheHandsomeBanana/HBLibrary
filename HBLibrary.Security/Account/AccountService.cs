using HBLibrary.Core;
using HBLibrary.Interface.Security.Account;
using HBLibrary.Interface.Security.Authentication;
using HBLibrary.Security;
using HBLibrary.Security.Authentication;
using System.Runtime.Versioning;
using System.Security;

namespace HBLibrary.Security.Account;

#if NET5_0_OR_GREATER
[SupportedOSPlatform("windows")]
#endif
public class AccountService : IAccountService {
    private readonly ILocalAuthenticationService localAuthService;
    private readonly IPublicMSAuthenticationService msAuthService;

    public bool IsLoggedIn { get; private set; }
    public IAccount? Account { get; private set; }
    public IAccountStorage AccountStorage { get; }

    public AccountService(ILocalAuthenticationService localAuthService,
        IPublicMSAuthenticationService msAuthService,
        IAccountStorage accountStorage) {

        this.localAuthService = localAuthService;
        this.msAuthService = msAuthService;
        AccountStorage = accountStorage;
    }

    public async Task LoginAsync(IAuthCredentials credentials, string application, CancellationToken cancellationToken = default) {
        switch (credentials) {
            case LocalAuthCredentials localCredentials:
                ILocalAuthResult localResult = await localAuthService.AuthenticateAsync(localCredentials, cancellationToken);

                Account = new LocalAccount {
                    Salt = localResult.Salt,
                    Application = application,
                    Username = localResult.Username,
                    PublicKey = localResult.PublicKey!,
                    SupportKey = localResult.SupportKey
                };

                break;
            case MSAuthCredentials msCredentials:
                IMSAuthResult msResult = await msAuthService.AuthenticateAsync(msCredentials, cancellationToken);

                Account = new MicrosoftAccount {
                    Application = application,
                    AccessToken = msResult.Result!.AccessToken,
                    Identifier = msResult.Result!.Account.HomeAccountId.Identifier,
                    UserId = msResult.UserId,
                    TenantId = msResult.Result!.TenantId,
                    Account = msResult.Result!.Account,
                    Username = msResult.Result!.Account.Username,
                    DisplayName = msResult.DisplayName,
                    Email = msResult.Email,
                    PublicKey = msResult.PublicKey,
                    Salt = msResult.Salt,
                    SupportKey = msResult.SupportKey
                };
                break;
            default:
                throw new NotSupportedException("Credential instance not supported");
        }

        IsLoggedIn = true;

        IAccountInfo accountInfo = await AccountStorage.GetUpdatedOrNewAsync(Account.AccountId,
            application,
            Account.CreateNewAccountInfo,
            cancellationToken);

        await AccountStorage.AddOrUpdateAccountAsync(accountInfo, cancellationToken);
    }

    public async Task RegisterAsync(IAuthCredentials credentials, string application, CancellationToken cancellationToken = default) {
        switch (credentials) {
            case LocalAuthCredentials localCredentials:
                ILocalAuthResult localResult = await localAuthService.AuthenticateNewAsync(localCredentials, cancellationToken);
                SecureString supportKey = KeyDerivation.DeriveNewSecureString(localCredentials.Password, GlobalEnvironment.Encoding.GetBytes(localResult.Salt));


                Account = new LocalAccount {
                    Salt = localResult.Salt,
                    Application = application,
                    Username = localResult.Username,
                    PublicKey = localResult.PublicKey,
                    SupportKey = supportKey
                };

                break;
            case MSAuthCredentials msCredentials:
                IMSAuthResult msResult = await msAuthService.AuthenticateAsync(msCredentials, cancellationToken);

                string saltString = $"{msResult.Result!.Account.Username}.{msResult.Email}";
                byte[] salt = GlobalEnvironment.Encoding.GetBytes(saltString);
                string temp = $"{msResult.Result!.TenantId}.{msResult.Result.Account.HomeAccountId.Identifier}";
                supportKey = KeyDerivation.DeriveNewSecureString(temp, salt);


                Account = new MicrosoftAccount {
                    Application = application,
                    PublicKey = msResult.PublicKey,
                    SupportKey = supportKey,
                    AccessToken = msResult.Result!.AccessToken,
                    UserId = msResult.UserId,
                    Identifier = msResult.Result!.Account.HomeAccountId.Identifier,
                    TenantId = msResult.Result!.TenantId,
                    Account = msResult.Result!.Account,
                    Username = msResult.Result!.Account.Username,
                    DisplayName = msResult.DisplayName,
                    Email = msResult.Email,
                    Salt = saltString
                };
                break;
            default:
                throw new NotSupportedException("Credential instance not supported");
        }

        IsLoggedIn = true;

        IAccountInfo accountInfo = Account.CreateNewAccountInfo();
        await AccountStorage.AddOrUpdateAccountAsync(accountInfo, cancellationToken);
    }


    public async Task LogoutAsync(CancellationToken cancellationToken = default) {

        switch (Account) {
            case MicrosoftAccount msAccount:
                await msAuthService.SignOutAsync(msAccount.Account!, cancellationToken);
                IsLoggedIn = false;

                await AccountStorage.RemoveApplicationFromAccountAsync(Account.AccountId, Account.Application, cancellationToken);
                break;
            case LocalAccount localAccount:
                await localAuthService.DeleteLocalUser(localAccount.Username, cancellationToken);
                IsLoggedIn = false;

                await AccountStorage.RemoveAccountAsync(Account.AccountId, cancellationToken);
                break;
        }
    }
}
