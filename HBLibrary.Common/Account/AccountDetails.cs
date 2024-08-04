using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Account;
public abstract class AccountDetails {
    public required string Token { get; init; }

}

public class LocalAccountDetails : AccountDetails {
    public required string Username { get; init; }
}


public class MicrosoftAccountDetails : AccountDetails {
    public required IAccount Account { get; init; }
    public required string TenantId { get; init; }
}