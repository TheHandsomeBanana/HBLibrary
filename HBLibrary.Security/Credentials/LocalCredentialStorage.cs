using HBLibrary.Core.Extensions;
using System.Runtime.Versioning;
using System.Security;
using System.Text.Json;
#if WINDOWS
namespace HBLibrary.Security.Credentials;

#if NET5_0_OR_GREATER
[SupportedOSPlatform("windows")]
#endif
public class LocalCredentialStorage {
    private readonly string appCredentialPath;

    public LocalCredentialStorage() {
        this.appCredentialPath = Path.Combine(GlobalEnvironment.IdentityPath, "credentialstorage");
        if (!File.Exists(appCredentialPath)) {
            File.Create(appCredentialPath).Dispose();
        }
    }

    public UserCredentials RegisterUser(string username, SecureString password) {
        if (GetUserCredentials(username) is not null) {
            throw new InvalidOperationException("User already registered");
        }

        byte[] salt = KeyDerivation.GenerateSalt(16);
        byte[] hashedPassword = KeyDerivation.DeriveKey(password, salt, 10000, 32);
        UserCredentials credentials = new UserCredentials {
            Username = username,
            PasswordHash = Convert.ToBase64String(hashedPassword),
            Salt = Convert.ToBase64String(salt)
        };

        List<UserCredentials> credentialsList = LoadCredentials();
        credentialsList.Add(credentials);
        SaveCredentials(credentialsList);
        return credentials;
    }

    public async Task<UserCredentials> RegisterUserAsync(string username, SecureString password, CancellationToken cancellationToken = default) {
        UserCredentials? existingUserCredentials = await GetUserCredentialsAsync(username, cancellationToken);
        if (existingUserCredentials is not null) {
            return existingUserCredentials;
        }

        byte[] salt = KeyDerivation.GenerateSalt(16);
        byte[] hashedPassword = KeyDerivation.DeriveKey(password, salt, 10000, 32);
        UserCredentials credentials = new UserCredentials {
            Username = username,
            PasswordHash = Convert.ToBase64String(hashedPassword),
            Salt = Convert.ToBase64String(salt)
        };

        List<UserCredentials> credentialsList = await LoadCredentialsAsync(cancellationToken);
        credentialsList.Add(credentials);
        await SaveCredentialsAsync(credentialsList, cancellationToken);
        return credentials;
    }

    public async Task UnregisterUserAsync(string username, CancellationToken cancellationToken = default) {
        UserCredentials? existingUserCredentials = await GetUserCredentialsAsync(username, cancellationToken);
        if (existingUserCredentials is null) {
            return;
        }

        List<UserCredentials> credentialsList = await LoadCredentialsAsync(cancellationToken);
        credentialsList.Remove(existingUserCredentials);
        await SaveCredentialsAsync(credentialsList, cancellationToken);
    }

    public void UnregisterUser(string username) {
        UserCredentials? existingUserCredentials = GetUserCredentials(username);
        if (existingUserCredentials is null) {
            return;
        }

        List<UserCredentials> credentialsList = LoadCredentials();
        credentialsList.Remove(existingUserCredentials);
        SaveCredentials(credentialsList);
    }

    public UserCredentials? GetUserCredentials(string username) {
        return LoadCredentials().FirstOrDefault(e => e.Username == username);
    }

    public async Task<UserCredentials?> GetUserCredentialsAsync(string username, CancellationToken cancellationToken = default) {
        return (await LoadCredentialsAsync(cancellationToken)).FirstOrDefault(e => e.Username == username);
    }

    private void SaveCredentials(List<UserCredentials> credentialsList) {
        string json = JsonSerializer.Serialize(credentialsList);
        byte[] encryptedJson = DPApi.Protect(GlobalEnvironment.Encoding.GetBytes(json));
        File.WriteAllBytes(appCredentialPath, encryptedJson);
    }

    private List<UserCredentials> LoadCredentials() {
        byte[] encryptedJson = File.ReadAllBytes(appCredentialPath);
        string json = GlobalEnvironment.Encoding.GetString(DPApi.Unprotect(encryptedJson));
        return JsonSerializer.Deserialize<List<UserCredentials>>(json) ?? [];
    }


    private Task SaveCredentialsAsync(List<UserCredentials> credentialsList, CancellationToken cancellationToken = default) {
        string json = JsonSerializer.Serialize(credentialsList);
        byte[] encryptedJson = DPApi.Protect(GlobalEnvironment.Encoding.GetBytes(json));
#if NET5_0_OR_GREATER
        return File.WriteAllBytesAsync(appCredentialPath, encryptedJson, cancellationToken);
#elif NET472_OR_GREATER
        using (FileStream fs = new FileStream(appCredentialPath, FileMode.Open, FileAccess.Read)) {
            return fs.WriteAsync(encryptedJson, cancellationToken);
        }
#endif
    }

    private async Task<List<UserCredentials>> LoadCredentialsAsync(CancellationToken cancellationToken = default) {
        byte[] encryptedJson;

#if NET5_0_OR_GREATER
        encryptedJson = await File.ReadAllBytesAsync(appCredentialPath, cancellationToken);
#elif NET472_OR_GREATER
        using (FileStream fs = new FileStream(appCredentialPath, FileMode.Open, FileAccess.Read)) {
            encryptedJson = await fs.ReadAsync(cancellationToken);
        }
#endif

        if (encryptedJson.Length == 0) {
            return [];
        }

        string json = GlobalEnvironment.Encoding.GetString(DPApi.Unprotect(encryptedJson));
        if (string.IsNullOrWhiteSpace(json)) {
            return [];
        }

        return JsonSerializer.Deserialize<List<UserCredentials>>(json) ?? [];
    }
}
#endif