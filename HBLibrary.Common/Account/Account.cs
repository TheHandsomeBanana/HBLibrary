using Microsoft.Identity.Client;

namespace HBLibrary.Common.Account;

public abstract class Account {
    public abstract AccountType AccountType { get; }
    public required string Token { get; set; }
    public required string Application { get; set; }
    public required string Username { get; set; }

    public abstract string AccountId { get; }

    public ApplicationAccountInfo GetApplicationAccountInfo() => new ApplicationAccountInfo {
        Application = Application,
        AccountType = AccountType,
        Username = Username
    };
}

public class LocalAccount : Account {
    public override AccountType AccountType => AccountType.Local;

    public override string AccountId => Username;
}


public class MicrosoftAccount : Account {
    public override AccountType AccountType => AccountType.Microsoft;
    public IAccount? Account { get; set; }
    public required string Identifier { get; set; }
    public required string Email { get; set; }
    public required string DisplayName { get; set; }
    public required string TenantId { get; set; }

    public override string AccountId => Identifier;

    public async Task SetAccountAsync(IPublicClientApplication app) {
        Account = await app.GetAccountAsync(Identifier);
    }
}

public enum AccountType {
    Local,
    Microsoft
}