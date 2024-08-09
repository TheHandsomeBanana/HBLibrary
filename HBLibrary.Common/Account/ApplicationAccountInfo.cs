using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Account;
public class ApplicationAccountInfo : IEquatable<ApplicationAccountInfo> {
    public AccountType AccountType { get; set; }
    public required string Application { get; set; }
    public required string Username { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }

    public bool Equals(ApplicationAccountInfo? other) {
        return other is not null
            && AccountType == other.AccountType
            && Application == other.Application
            && Username == other.Username;
    }

    public override bool Equals(object? obj) {
        return Equals(obj as ApplicationAccountInfo);
    }

    public override int GetHashCode() {
        return HBHashCode.Combine(AccountType, Application, Username);
    }
}
