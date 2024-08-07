using HBLibrary.Common.Extensions;
using HBLibrary.Common.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace HBLibrary.Common.Authentication.Microsoft;
public class MSParameterStorage {
    private readonly string appMSParameterPath;
    public MSParameterStorage(string appName) {
        this.appMSParameterPath = Path.Combine(GlobalEnvironment.IdentityPath, appName + ".msparams");
        if (!File.Exists(appMSParameterPath)) {
            File.Create(appMSParameterPath).Dispose();
        }
    }

    public async Task<MicrosoftIdentity> RegisterIdentityAsync(string username, string identifier, string email, string displayName, string[] scopes, string tenantId, CancellationToken cancellationToken = default) {
        MicrosoftIdentity? existingIdentity = await GetIdentityAsync(username, cancellationToken);
        if (existingIdentity is not null) {
            return existingIdentity;
        }

        MicrosoftIdentity msIdentity = new MicrosoftIdentity {
            Identifier = identifier,
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

    public async Task<MicrosoftIdentity?> GetIdentityAsync(string username, CancellationToken cancellationToken = default) {
        return (await LoadIdentitiesAsync(cancellationToken)).FirstOrDefault(e => e.Username == username);
    }
    
    public async Task<MicrosoftIdentity?> GetIdentityByIdAsync(string identifier, CancellationToken cancellationToken = default) {
        return (await LoadIdentitiesAsync(cancellationToken)).FirstOrDefault(e => e.Identifier == identifier);
    }

    private Task SaveCredentialsAsync(List<MicrosoftIdentity> identityList, CancellationToken cancellationToken = default) {
        string json = JsonSerializer.Serialize(identityList);
        string base64Json = Convert.ToBase64String(GlobalEnvironment.Encoding.GetBytes(json));

#if NET5_0_OR_GREATER
        return File.WriteAllTextAsync(appMSParameterPath, base64Json, cancellationToken);
#elif NET472_OR_GREATER
        using (FileStream fs = new FileStream(appMSParameterPath, FileMode.Open, FileAccess.Read)) {
            using (StreamWriter sw = new StreamWriter(fs)) {
                return sw.WriteAsync(base64Json);
            }
        }
#endif
    }

    private async Task<List<MicrosoftIdentity>> LoadIdentitiesAsync(CancellationToken cancellationToken = default) {
        string base64Json;
#if NET5_0_OR_GREATER
        base64Json = await File.ReadAllTextAsync(appMSParameterPath, cancellationToken);
#elif NET472_OR_GREATER
        using(FileStream fs = new FileStream(appMSParameterPath, FileMode.Open, FileAccess.Read)) {
            using(StreamReader sr = new StreamReader(fs)) {           
                base64Json = await sr.ReadToEndAsync();
            }
        }
#endif

        string json = GlobalEnvironment.Encoding.GetString(Convert.FromBase64String(base64Json));
        return JsonSerializer.Deserialize<List<MicrosoftIdentity>>(json) ?? [];
    }
}
