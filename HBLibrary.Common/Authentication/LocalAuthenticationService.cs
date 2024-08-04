using HBLibrary.Common.Account;
using HBLibrary.Common.Security;
using HBLibrary.Common.Security.Credentials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Authentication;
#if WINDOWS
#if NET5_0_OR_GREATER
[SupportedOSPlatform("windows")]
#endif
public sealed class LocalAuthenticationService : IAuthenticationService<LocalAuthResult, LocalAuthCredentials> {
    private readonly CredentialStorage credentialStorage;
    private readonly IAccountService? accountService;
    public LocalAuthenticationService(CredentialStorage credentialStorage, IAccountService? accountService = null) {
        this.credentialStorage = credentialStorage;
        this.accountService = accountService;
    }

    public LocalAuthenticationService(string applicationName, IAccountService? accountService = null) {
        this.credentialStorage = new CredentialStorage(applicationName);
        this.accountService = accountService;
    }

    public Task<LocalAuthResult> Authenticate(LocalAuthCredentials authCredentials) {
        if (IsNewUser(authCredentials.Username)) {
            credentialStorage.RegisterUser(authCredentials.Username, authCredentials.Password);
        }
        else if(!VerifyCredentials(authCredentials.Username, authCredentials.Password)) {
            throw new InvalidOperationException("Password is invalid.");
        }

        string token = CreateEncryptionKey(authCredentials.Password);

        accountService?.SetCurrentAccount(new LocalAccountDetails { 
            Username = authCredentials.Username,
            Token = token
        });

        return Task.FromResult(new LocalAuthResult {
            Token = token,
            Username = authCredentials.Username
        });
    }

    private bool IsNewUser(string username) {
        return credentialStorage.GetUserCredentials(username) is null;
    }

    private bool VerifyCredentials(string username, SecureString password) {
        UserCredentials? credentials = credentialStorage.GetUserCredentials(username);
        if (credentials is null) {
            return false;
        }

        byte[] salt = Convert.FromBase64String(credentials.Salt);
        byte[] storedHashedPassword = Convert.FromBase64String(credentials.Password);
        byte[] hashedPassword = KeyDerivation.DeriveKey(password, salt, 10000, 32);

        return hashedPassword.SequenceEqual(storedHashedPassword);
    }

    private string CreateEncryptionKey(SecureString password) {
        byte[] salt = KeyDerivation.GenerateSalt(16);
        byte[] key = KeyDerivation.DeriveKey(password, salt, 20000, 32);

        string saltBase64 = Convert.ToBase64String(salt);
        string keyBase64 = Convert.ToBase64String(key);
        return $"{saltBase64}:{keyBase64}";
    }
}
#endif