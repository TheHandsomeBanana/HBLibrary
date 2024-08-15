using HBLibrary.Common.Exceptions;
using HBLibrary.Common.Security;
using HBLibrary.Common.Security.Credentials;
using System.Runtime.Versioning;
using System.Security;

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
            AuthenticationException.ThrowUserNotFound();
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

    public async Task<bool> IsNewUserAsync(string username, CancellationToken cancellationToken = default) {
        return (await credentialStorage.GetUserCredentialsAsync(username, cancellationToken)) is null;
    }

    public Task DeleteLocalUser(string username, CancellationToken cancellationToken = default) {
        return credentialStorage.UnregisterUserAsync(username, cancellationToken);
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

    public async Task<LocalAuthResult> AuthenticateNewAsync(LocalAuthCredentials authCredentials, CancellationToken cancellationToken = default) {
        if (await IsNewUserAsync(authCredentials.Username, cancellationToken)) {
            UserCredentials credentials = await credentialStorage.RegisterUserAsync(authCredentials.Username, authCredentials.Password, cancellationToken);

            string token = CreateEncryptionKey(authCredentials.Password);

            return new LocalAuthResult {
                Token = token,
                Username = credentials.Username,
            };
        }

        throw AuthenticationException.UserAlreadyExists();
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