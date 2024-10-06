using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HBLibrary.Common.Workspace;
public class WorkspaceLocationCache {
    private readonly string application;
    public WorkspaceLocationCache(string application) {
        this.application = application;
    }

    public async Task<List<string>> LoadCachedWorkspaceLocationsAsync() {
        string filepath = Path.Combine(GlobalEnvironment.ApplicationDataBasePath,
            application,
            "workspacecache");

        string cachedWorkspaceLocations = await UnifiedFile.ReadAllTextAsync(filepath);
        return JsonSerializer.Deserialize<List<string>>(cachedWorkspaceLocations)!;
    }

    public async Task AddCachedWorkspaceLocationAsync(string workspaceLocation) {
        List<string> cachedLocations = await LoadCachedWorkspaceLocationsAsync();
        cachedLocations.Add(workspaceLocation);

        await SaveCachedWorkspaceLocationAsync(cachedLocations);
    }

    public async Task RemoveCachedWorkspaceLocationAsync(string workspaceLocation) {
        List<string> cachedLocations = await LoadCachedWorkspaceLocationsAsync();
        cachedLocations.Remove(workspaceLocation);

        await SaveCachedWorkspaceLocationAsync(cachedLocations);
    }

    private Task SaveCachedWorkspaceLocationAsync(List<string> cachedLocations) {
        string filePath = Path.Combine(GlobalEnvironment.ApplicationDataBasePath,
            application,
            "workspacecache");

        string serializedLocations = JsonSerializer.Serialize(cachedLocations);

        return UnifiedFile.WriteAllTextAsync(filePath, serializedLocations);
    }
}
