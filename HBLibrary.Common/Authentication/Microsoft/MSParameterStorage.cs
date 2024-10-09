using System.Text.Json;
using System.Threading;

namespace HBLibrary.Common.Authentication.Microsoft;
public class MSParameterStorage {
    private readonly string appMSParameterPath;
    public MSParameterStorage() {
        this.appMSParameterPath = Path.Combine(GlobalEnvironment.IdentityPath, "msaccountidentity");
        if (!File.Exists(appMSParameterPath)) {
            File.Create(appMSParameterPath).Dispose();
        }
    }

    public async Task<bool> IdentityExistsAsync(string username, CancellationToken cancellationToken = default) {
        MicrosoftIdentity? existingIdentity = await GetIdentityAsync(username, cancellationToken);
        return existingIdentity is not null;
    }

    public async Task<MicrosoftIdentity> RegisterIdentityAsync(string username, string identifier, string userId, string email, string displayName, string[] scopes, string tenantId, CancellationToken cancellationToken = default) {
        MicrosoftIdentity? existingIdentity = await GetIdentityAsync(username, cancellationToken);
        if (existingIdentity is not null) {
            return existingIdentity;
        }

        MicrosoftIdentity msIdentity = new MicrosoftIdentity {
            Identifier = identifier,
            UserId = userId,
            Username = username,
            Email = email,
            DisplayName = displayName,
            Scopes = scopes,
            TenantId = tenantId
        };

        List<MicrosoftIdentity> identityList = await LoadIdentitiesAsync(cancellationToken);
        identityList.Add(msIdentity);
        await SaveCredentialsAsync(identityList, cancellationToken);
        return msIdentity;
    }

    public async Task UnregisterIdentityAsync(string username, CancellationToken cancellationToken = default) {
        MicrosoftIdentity? existingIdentity = await GetIdentityAsync(username, cancellationToken);
        if (existingIdentity is null) {
            return;
        }

        List<MicrosoftIdentity> identityList = await LoadIdentitiesAsync(cancellationToken);
        identityList.Remove(existingIdentity);
        await SaveCredentialsAsync(identityList, cancellationToken);
    }

    public async Task UnregisterIdentityByIdAsync(string identifier, CancellationToken cancellationToken = default) {
        MicrosoftIdentity? existingIdentity = await GetIdentityByIdAsync(identifier, cancellationToken);
        if (existingIdentity is null) {
            return;
        }

        List<MicrosoftIdentity> identityList = await LoadIdentitiesAsync(cancellationToken);
        identityList.Remove(existingIdentity);
        await SaveCredentialsAsync(identityList, cancellationToken);
    }

    public MicrosoftIdentity? GetIdentity(string username) {
        return LoadIdentities().FirstOrDefault(e => e.Username == username);
    }

    public async Task<MicrosoftIdentity?> GetIdentityAsync(string username, CancellationToken cancellationToken = default) {
        return (await LoadIdentitiesAsync(cancellationToken)).FirstOrDefault(e => e.Username == username);
    }

    public async Task<MicrosoftIdentity?> GetIdentityByIdAsync(string identifier, CancellationToken cancellationToken = default) {
        return (await LoadIdentitiesAsync(cancellationToken)).FirstOrDefault(e => e.Identifier == identifier);
    }

    private Task SaveCredentialsAsync(List<MicrosoftIdentity> identityList, CancellationToken cancellationToken = default) {
        string json = JsonSerializer.Serialize(identityList);
        string base64Json = Convert.ToBase64String(GlobalEnvironment.Encoding.GetBytes(json));

        return UnifiedFile.WriteAllTextAsync(appMSParameterPath, base64Json, cancellationToken);
    }

    private List<MicrosoftIdentity> LoadIdentities() {
        string base64Json = File.ReadAllText(appMSParameterPath);
        string json = GlobalEnvironment.Encoding.GetString(Convert.FromBase64String(base64Json));
        if (string.IsNullOrWhiteSpace(json)) {
            return [];
        }

        return JsonSerializer.Deserialize<List<MicrosoftIdentity>>(json) ?? [];
    }

    private async Task<List<MicrosoftIdentity>> LoadIdentitiesAsync(CancellationToken cancellationToken = default) {
        string base64Json = await UnifiedFile.ReadAllTextAsync(appMSParameterPath, cancellationToken);

        string json = GlobalEnvironment.Encoding.GetString(Convert.FromBase64String(base64Json));
        if (string.IsNullOrWhiteSpace(json)) {
            return [];
        }

        return JsonSerializer.Deserialize<List<MicrosoftIdentity>>(json) ?? [];
    }
}
