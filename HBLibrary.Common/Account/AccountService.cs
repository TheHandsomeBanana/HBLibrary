using HBLibrary.Common.Authentication;

namespace HBLibrary.Common.Account;
public class AccountService : IAccountService {
    private readonly ILocalAuthenticationService localAuthService;
    private readonly IPublicMSAuthenticationService msAuthService;

    public bool IsLoggedIn { get; private set; }
    public Account? Account { get; private set; }

    public AccountService(ILocalAuthenticationService localAuthService, IPublicMSAuthenticationService msAuthService) {
        this.localAuthService = localAuthService;
        this.msAuthService = msAuthService;
    }

    public async Task LoginAsync(IAuthCredentials credentials, string application, CancellationToken cancellationToken = default) {
        switch (credentials) {
            case LocalAuthCredentials localCredentials:
                LocalAuthResult localResult = await localAuthService.AuthenticateAsync(localCredentials, cancellationToken);
                Account = new LocalAccount {
                    Application = application,
                    Username = localResult.Username,
                    Token = localResult.Token!
                };

                break;
            case MSAuthCredentials msCredentials:
                MSAuthResult msResult = await msAuthService.AuthenticateAsync(msCredentials, cancellationToken);

                Account = new MicrosoftAccount {
                    Application = application,
                    Token = msResult.Result!.AccessToken,
                    Identifier = msResult.Result!.Account.HomeAccountId.Identifier,
                    TenantId = msResult.Result!.TenantId,
                    Account = msResult.Result!.Account,
                    Username = msResult.Result!.Account.Username,
                    DisplayName = msResult.DisplayName,
                    Email = msResult.Email
                };
                break;
            default:
                throw new NotSupportedException("Credential instance not supported");
        }

        IsLoggedIn = true;

        await SaveCurrentAccountAsync();
    }

    public async Task RegisterAsync(IAuthCredentials credentials, string application, CancellationToken cancellationToken = default) {
        switch (credentials) {
            case LocalAuthCredentials localCredentials:
                LocalAuthResult localResult = await localAuthService.AuthenticateNewAsync(localCredentials, cancellationToken);
                Account = new LocalAccount {
                    Application = application,
                    Username = localResult.Username,
                    Token = localResult.Token!
                };

                break;
            case MSAuthCredentials msCredentials:
                MSAuthResult msResult = await msAuthService.AuthenticateAsync(msCredentials, cancellationToken);

                Account = new MicrosoftAccount {
                    Application = application,
                    Token = msResult.Result!.AccessToken,
                    Identifier = msResult.Result!.Account.HomeAccountId.Identifier,
                    TenantId = msResult.Result!.TenantId,
                    Account = msResult.Result!.Account,
                    Username = msResult.Result!.Account.Username,
                    DisplayName = msResult.DisplayName,
                    Email = msResult.Email
                };
                break;
            default:
                throw new NotSupportedException("Credential instance not supported");
        }

        IsLoggedIn = true;

        await SaveCurrentAccountAsync();
    }


    public async Task LogoutAsync(CancellationToken cancellationToken = default) {

        switch (Account) {
            case MicrosoftAccount msAccount:
                await msAuthService.SignOutAsync(msAccount.Account!, cancellationToken);
                IsLoggedIn = false;
                ApplicationAccountInfo accountInfo = Account.GetApplicationAccountInfo();
                accountInfo.Username = "";
                await SaveAccountAsync(accountInfo);
                break;
            case LocalAccount localAccount:
                await localAuthService.DeleteLocalUser(localAccount.Username, cancellationToken);
                IsLoggedIn = false;

                accountInfo = Account.GetApplicationAccountInfo();
                accountInfo.Username = "";
                await SaveAccountAsync(accountInfo);
                break;
        }
    }

    public Task<ApplicationAccountInfo?> GetLastAccountAsync(string application, CancellationToken cancellationToken = default) {
        AccountStorage accountStorage = new AccountStorage();
        return accountStorage.GetAccountAsync(application);
    }

    public Task SaveAccountAsync(ApplicationAccountInfo accountInfo) {
        AccountStorage accountStorage = new AccountStorage();
        return accountStorage.AddOrUpdateAccountAsync(accountInfo);
    }

    public ApplicationAccountInfo? GetLastAccount(string application) {
        AccountStorage accountStorage = new AccountStorage();
        return accountStorage.GetAccount(application);
    }

    public void SaveAccount(ApplicationAccountInfo accountInfo) {
        AccountStorage accountStorage = new AccountStorage();
        accountStorage.AddOrUpdateAccount(accountInfo);
    }

    private Task SaveCurrentAccountAsync() {
        if (!IsLoggedIn) {
            throw new InvalidOperationException("Not logged in.");
        }

        AccountStorage accountStorage = new AccountStorage();
        ApplicationAccountInfo accountInfo = Account!.GetApplicationAccountInfo();

        return SaveAccountAsync(accountInfo);
    }

    private void SaveCurrentAccount() {
        if (!IsLoggedIn) {
            throw new InvalidOperationException("Not logged in.");
        }

        AccountStorage accountStorage = new AccountStorage();
        ApplicationAccountInfo accountInfo = Account!.GetApplicationAccountInfo();

        SaveAccount(accountInfo);
    }
}
