using HBLibrary.Common.Account;
using HBLibrary.Common.Exceptions;
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
public sealed class LocalAuthenticationService : ILocalAuthenticationService {
    private readonly LocalCredentialStorage credentialStorage;


    public LocalAuthenticationService(LocalCredentialStorage credentialStorage) {
        this.credentialStorage = credentialStorage;
    }

    public LocalAuthenticationService(string applicationName) {
        this.credentialStorage = new LocalCredentialStorage(applicationName);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="authCredentials"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="AuthenticationException"></exception>
    /// <returns></returns>
    public async Task<LocalAuthResult> AuthenticateAsync(LocalAuthCredentials authCredentials, CancellationToken cancellationToken = default) {
        if (await IsNewUserAsync(authCredentials.Username, cancellationToken)) {
            await credentialStorage.RegisterUserAsync(authCredentials.Username, authCredentials.Password, cancellationToken);
        }
        else if (!await VerifyCredentialsAsync(authCredentials.Username, authCredentials.Password, cancellationToken)) {
            AuthenticationException.ThrowInvalidCredentials();
        }

        string token = CreateEncryptionKey(authCredentials.Password);

        return new LocalAuthResult {
            Token = token,
            Username = authCredentials.Username,
        };
    }

    private async Task<bool> IsNewUserAsync(string username, CancellationToken cancellationToken = default) {
        return (await credentialStorage.GetUserCredentialsAsync(username, cancellationToken)) is null;
    }

    private async Task<bool> VerifyCredentialsAsync(string username, SecureString password, CancellationToken cancellationToken = default) {
        UserCredentials? credentials = await credentialStorage.GetUserCredentialsAsync(username, cancellationToken);
        if (credentials is null) {
            return false;
        }

        byte[] salt = Convert.FromBase64String(credentials.Salt);
        byte[] storedHashedPassword = Convert.FromBase64String(credentials.PasswordHash);
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