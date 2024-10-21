using HBLibrary.Core;
using HBLibrary.DataStructures;
using HBLibrary.Interface.Security.Account;
using HBLibrary.Interface.Security.Keys;
using HBLibrary.Security;
using Microsoft.Identity.Client;
using System.Runtime.Versioning;
using System.Security;

namespace HBLibrary.Security.Account;
public abstract class Account : Interface.Security.Account.IAccount {
    public abstract AccountType AccountType { get; }
    public required string Application { get; set; }
    public required string Username { get; set; }
    public required RsaKey PublicKey { get; set; }
    public required string Salt { get; set; }
    public required SecureString SupportKey { get; set; }

    public abstract string AccountId { get; }

    public IAccountInfo CreateNewAccountInfo() => new AccountInfo {
        AccountType = AccountType,
        Username = Username,
        AccountId = AccountId,
        Applications = [new AccountApplicationInfo {
            Application = Application,
            LastLogin = DateTime.UtcNow,
            InitialLogin = DateTime.UtcNow,
        }],
        CreatedOn = DateTime.UtcNow,
        ModifiedOn = DateTime.UtcNow,
    };

    public async Task<Result<RsaKey>> GetPrivateKeyAsync() {
        AccountKeyManager accountKeyManager = new AccountKeyManager();
        Result<RsaKey> keyResult = await accountKeyManager.GetPrivateKeyAsync(AccountId, SupportKey, GlobalEnvironment.Encoding.GetBytes(Salt));
        return keyResult;
    }

    public Result<RsaKey> GetPrivateKey() {
        AccountKeyManager accountKeyManager = new AccountKeyManager();
        Result<RsaKey> keyResult = accountKeyManager.GetPrivateKey(AccountId, SupportKey, GlobalEnvironment.Encoding.GetBytes(Salt));
        return keyResult;
    }
}


public class LocalAccount : Account {
    public override AccountType AccountType => AccountType.Local;

    public override string AccountId => Username;
}

public class MicrosoftAccount : Account {
    public override AccountType AccountType => AccountType.Microsoft;
    public Microsoft.Identity.Client.IAccount? Account { get; set; }
    public required string Identifier { get; set; }
    public required string UserId { get; set; }
    public required string Email { get; set; }
    public required string DisplayName { get; set; }
    public required string TenantId { get; set; }
    public required string AccessToken { get; set; }
    public override string AccountId => UserId;

    public async Task SetAccountAsync(IPublicClientApplication app) {
        Account = await app.GetAccountAsync(Identifier);
    }
}

