using HBLibrary.Common.Security;
using HBLibrary.Common.Security.Credentials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Authentication;
#if WINDOWS
#if NET5_0_OR_GREATER
[SupportedOSPlatform("windows")]
#endif
public class LocalAuthenticationService {
    private readonly CredentialStorage credentialStorage;
    public LocalAuthenticationService(CredentialStorage credentialStorage) {
        this.credentialStorage = credentialStorage;
    }

    public LocalAuthenticationService(string applicationName) {
        this.credentialStorage = new CredentialStorage(applicationName);
    }

    public string Authenticate(string username, string password) {
        if(IsNewUser(username)) {
            credentialStorage.RegisterUser(username, password);
            return CreateEncryptionKey(username, password);
        }

        if(!VerifyCredentials(username, password)) {
            throw new InvalidOperationException("Password is invalid.");
        }

        return CreateEncryptionKey(username, password);
    }

    private bool IsNewUser(string username) {
        return credentialStorage.GetUserCredentials(username) is null;
    }

    private bool VerifyCredentials(string username, string password) {
        UserCredentials? credentials = credentialStorage.GetUserCredentials(username);
        if (credentials is null) {
            return false;
        }

        byte[] salt = Convert.FromBase64String(credentials.Salt);
        byte[] storedHashedPassword = Convert.FromBase64String(credentials.Password);
        byte[] hashedPassword = KeyDerivation.DeriveKey(password, salt, 10000, 32);

        return hashedPassword.SequenceEqual(storedHashedPassword);
    }

    private string CreateEncryptionKey(string username, string password) {
        byte[] salt = KeyDerivation.GenerateSalt(16);
        byte[] key = KeyDerivation.DeriveKey(username + password, salt, 20000, 32);

        string saltBase64 = Convert.ToBase64String(salt);
        string keyBase64 = Convert.ToBase64String(key);
        return $"{saltBase64}:{keyBase64}";
    }
}
#endif