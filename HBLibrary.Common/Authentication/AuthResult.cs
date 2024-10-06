using HBLibrary.Common.Security.Keys;
using Microsoft.Identity.Client;

namespace HBLibrary.Common.Authentication;
public abstract class AuthResult {
    public required RsaKey PublicKey { get; set; }
}

public sealed class LocalAuthResult : AuthResult {
    public required string Username { get; init; }
    public required string Salt { get; init; }
}

public sealed class MSAuthResult : AuthResult {
    public required AuthenticationResult? Result { get; init; }
    public required string DisplayName { get; init; }
    public required string Email { get; init; }
}

