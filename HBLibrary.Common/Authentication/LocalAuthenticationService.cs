using HBLibrary.Common.Exceptions;
using HBLibrary.Common.Security;
using HBLibrary.Common.Security.Credentials;
using HBLibrary.Common.Security.Keys;
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

    public LocalAuthenticationService() {
        this.credentialStorage = new LocalCredentialStorage();
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

        UserCredentials credentials = (await credentialStorage.GetUserCredentialsAsync(authCredentials.Username))!;

        AccountKeyManager accountKeyManager = new AccountKeyManager();
        Result<RsaKey> publicKeyResult = await accountKeyManager.GetPublicKeyAsync(authCredentials.Username);

        RsaKey publicKey = publicKeyResult.GetValueOrThrow(); 

        return new LocalAuthResult {
            Salt = credentials.Salt,
            Username = authCredentials.Username,
            PublicKey = publicKey
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

            AccountKeyManager accountKeyManager = new AccountKeyManager();
            byte[] salt = GlobalEnvironment.Encoding.GetBytes(credentials.Salt);
            Result<RsaKeyPair> keyPair = await accountKeyManager.CreateAccountKeysAsync(credentials.Username, salt);

            RsaKey publicKey = keyPair.GetValueOrThrow().PublicKey;

            return new LocalAuthResult {
                PublicKey = publicKey,
                Username = credentials.Username,
                Salt = credentials.Salt
            };
        }

        throw AuthenticationException.UserAlreadyExists();
    }
}
#endif