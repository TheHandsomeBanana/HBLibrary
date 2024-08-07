﻿using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Authentication;
public interface IAuthResult {

}

public sealed class LocalAuthResult : IAuthResult {
    public required string Username { get; init; }
    public required string Token { get; init; }
}

public sealed class MSAuthResult : IAuthResult {
    public required AuthenticationResult? Result { get; init; }
    public required string DisplayName { get; init; }
    public required string Email { get; init; }
}

