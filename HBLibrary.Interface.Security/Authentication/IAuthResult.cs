using HBLibrary.Interface.Security.Keys;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Security.Authentication;
public interface IAuthResult {
    public RsaKey PublicKey { get; set; }
    public SecureString SupportKey { get; set; }
    public string Salt { get; init; }
}

public interface ILocalAuthResult : IAuthResult {
    public string Username { get; init; }
}

public interface IMSAuthResult : IAuthResult {
    public AuthenticationResult? Result { get; init; }
    public string DisplayName { get; init; }
    public string Email { get; init; }
    public string UserId { get; init; }
}


