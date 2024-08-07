using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HBLibrary.Common.Account;

public abstract class Account {
    public abstract AccountType AccountType { get; }
    public required string Token { get; set; }
    public required string Application { get; set; }
    public required string Username { get; set; }

    public AccountInfo GetAccountInfo() => new AccountInfo {
        Application = Application,
        AccountType = AccountType,
        Username = Username
    };
}

public class LocalAccount : Account {
    public override AccountType AccountType => AccountType.Local;
}


public class MicrosoftAccount : Account {
    public override AccountType AccountType => AccountType.Microsoft;
    public IAccount? Account { get; set; }
    public required string Identifier { get; set; }
    public required string Email { get; set; }
    public required string DisplayName { get; set; }
    public required string TenantId { get; set; }

    public async Task SetAccountAsync(IPublicClientApplication app) {
        Account = await app.GetAccountAsync(Identifier);
    }
}

public enum AccountType {
    Local,
    Microsoft
}