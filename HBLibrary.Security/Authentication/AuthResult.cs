using HBLibrary.Interface.Security.Authentication;
using HBLibrary.Interface.Security.Keys;
using Microsoft.Identity.Client;
using System.Security;

namespace HBLibrary.Security.Authentication;
public abstract class AuthResult : IAuthResult {
    public required RsaKey PublicKey { get; set; }
    public required SecureString SupportKey { get; set; }
    public required string Salt { get; init; }
}

public sealed class LocalAuthResult : AuthResult, ILocalAuthResult {
    public required string Username { get; init; }
}

public sealed class MSAuthResult : AuthResult, IMSAuthResult {
    public required AuthenticationResult? Result { get; init; }
    public required string DisplayName { get; init; }
    public required string Email { get; init; }
    public required string UserId { get; init; }
}

