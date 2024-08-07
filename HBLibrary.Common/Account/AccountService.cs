using HBLibrary.Common.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public async Task LogoutAsync(CancellationToken cancellationToken = default) {
        IsLoggedIn = false;

        if(Account is MicrosoftAccount msAccount) {
            await msAuthService.SignOutAsync(msAccount.Account!, cancellationToken);
        }
    }

    public Task SaveAccountAsync(AccountInfo accountInfo) {
        AccountStorage accountStorage = new AccountStorage();
        return accountStorage.AddOrUpdateAccountAsync(accountInfo);
    }

    public Task<AccountInfo?> GetLastAccountAsync(string application, CancellationToken cancellationToken = default) {
        Common.Account.AccountStorage accountStorage = new AccountStorage();
        return accountStorage.GetAccountAsync(application);
    }

    private Task SaveCurrentAccountAsync() {
        if(!IsLoggedIn) {
            throw new InvalidOperationException("Not logged in.");
        }

        AccountStorage accountStorage = new AccountStorage();
        AccountInfo accountInfo = Account!.GetAccountInfo();

        return SaveAccountAsync(accountInfo);
    }
}
