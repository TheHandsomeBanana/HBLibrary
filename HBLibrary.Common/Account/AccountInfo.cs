using HBLibrary.Common.Security;
using HBLibrary.Common.Security.Keys;
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


    public async Task<Result<RsaKey>> GetPublicKeyAsync() {
        AccountKeyManager accountKeyManager = new AccountKeyManager();
        Result<RsaKey> keyResult = await accountKeyManager.GetPublicKeyAsync(AccountId);
        return keyResult;
    }

    public Result<RsaKey> GetPublicKey() {
        AccountKeyManager accountKeyManager = new AccountKeyManager();
        Result<RsaKey> keyResult = accountKeyManager.GetPublicKey(AccountId);
        return keyResult;
    }

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
