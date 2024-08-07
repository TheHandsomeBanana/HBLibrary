using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Authentication.Microsoft;
public class MicrosoftIdentity : IEquatable<MicrosoftIdentity> {
    public required string Username { get; set; }
    public required string Identifier { get; set; }
    public required string Email { get; set; }
    public required string DisplayName { get; set; }
    public required string TenantId { get; set; }
    public required string[] Scopes { get; set; }

    public bool Equals(MicrosoftIdentity? other) {
        return other is not null 
            && Username == other.Username
            && Identifier == other.Identifier
            && Email == other.Email;
    }

    public override bool Equals(object? obj) {
        return Equals(obj as MicrosoftIdentity);
    }

    public override int GetHashCode() {
        return HBHashCode.Combine(Username, Identifier, Email);
    }
}
