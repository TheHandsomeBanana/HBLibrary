using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
#if WINDOWS
namespace HBLibrary.Common.Security.Credentials;

#if NET5_0_OR_GREATER
[SupportedOSPlatform("windows")]
#endif
public class CredentialStorage {
    private readonly string appCredentialPath;

    public CredentialStorage(string appName) {
        this.appCredentialPath = Path.Combine(GlobalEnvironment.IdentityPath, appName + ".creds");
        if (!File.Exists(appCredentialPath)) {
            File.Create(appCredentialPath).Dispose();
        }
    }

    public UserCredentials RegisterUser(string username, string password) {
        if (GetUserCredentials(username) is not null) {
            throw new InvalidOperationException("User already registered");
        }

        byte[] salt = KeyDerivation.GenerateSalt(16);
        byte[] hashedPassword = KeyDerivation.DeriveKey(password, salt, 10000, 32);
        UserCredentials credentials = new UserCredentials {
            Username = username,
            Password = Convert.ToBase64String(hashedPassword),
            Salt = Convert.ToBase64String(salt)
        };

        List<UserCredentials> credentialsList = LoadCredentials();
        credentialsList.Add(credentials);
        SaveCredentials(credentialsList);
        return credentials;
    }

    private void SaveCredentials(List<UserCredentials> credentialsList) {
        string json = JsonSerializer.Serialize(credentialsList);
        byte[] encryptedJson = DPApi.Protect(GlobalEnvironment.Encoding.GetBytes(json));
        File.WriteAllBytes(appCredentialPath, encryptedJson);
    }

    public UserCredentials? GetUserCredentials(string username) {
        return LoadCredentials().FirstOrDefault(e => e.Username == username);
    }

    private List<UserCredentials> LoadCredentials() {
        byte[] encryptedJson = File.ReadAllBytes(appCredentialPath);
        string json = GlobalEnvironment.Encoding.GetString(DPApi.Unprotect(encryptedJson));
        return JsonSerializer.Deserialize<List<UserCredentials>>(json) ?? [];
    }
}
#endif