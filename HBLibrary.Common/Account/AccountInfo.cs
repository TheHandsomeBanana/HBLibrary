using Microsoft.Graph.Models;
using Unity;

namespace HBLibrary.Common.Account;
public class AccountInfo : IEquatable<AccountInfo> {
    public AccountType AccountType { get; set; }
    public required string AccountId { get; set; }
    public required string Username { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
    public List<AccountApplicationInfo> Applications { get; set; } = [];
    

    public bool Equals(AccountInfo? other) {
        return other is not null && AccountId == other.AccountId;
    }

    public override bool Equals(object? obj) {
        return Equals(obj as AccountInfo);
    }

    public override int GetHashCode() {
        return AccountId.GetHashCode();
    }

    public static bool operator ==(AccountInfo? left, AccountInfo? right) {
        if (left is null && right is null)
            return true;

        return left!.Equals(right);
    }

    public static bool operator !=(AccountInfo? left, AccountInfo? right) {
        return !(left == right);
    }
}

public class AccountApplicationInfo {
    public required string Application { get; set; }
    public DateTime LastLogin { get; set; }
    public DateTime InitialLogin { get; set; }
}
