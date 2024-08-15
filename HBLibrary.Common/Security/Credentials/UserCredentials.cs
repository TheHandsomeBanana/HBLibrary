#if WINDOWS
namespace HBLibrary.Common.Security.Credentials;
public class UserCredentials : IEquatable<UserCredentials> {
    public required string Username { get; set; }
    public required string Salt { get; set; }
    public required string PasswordHash { get; set; }

    public bool Equals(UserCredentials? other) {
        return other is not null && Username == other.Username;
    }

    public override bool Equals(object? obj) {
        return Equals(obj as UserCredentials);
    }

    public override int GetHashCode() {
        return Username.GetHashCode();
    }
}
#endif