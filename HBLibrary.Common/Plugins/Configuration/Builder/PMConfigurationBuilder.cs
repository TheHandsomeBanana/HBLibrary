using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Plugins.Configuration.Builder;
internal class PMConfigurationBuilder : IPMConfigurationBuilder {
    private string? location;
    private bool cachePluginInstances = false;
    private bool preloadAssemblies = true;
    private bool overrideAssemblies = true;
    
    public IPMConfiguration Build() {
        if(location is null) {
            throw new InvalidOperationException("Location is not set.");
        }

        return new PMConfiguration() {
            Location = location,
            CachePluginInstances = cachePluginInstances,
            OverrideAssemblies = overrideAssemblies,
            PreloadAssemblies = preloadAssemblies
        };
    }

    public IPMConfigurationBuilder SetPluginsLocation(string location) {
        this.location = location;
        return this;
    }

    public IPMConfigurationBuilder CachePluginInstances(bool cachePluginInstances) {
        this.cachePluginInstances = cachePluginInstances;
        return this;
    }

    public IPMConfigurationBuilder PreloadAssemblies(bool preloadAssemblies) {
        this.preloadAssemblies = preloadAssemblies;
        return this;
    }

    public IPMConfigurationBuilder OverrideAssemblies(bool overrideAssemblies) {
        this.overrideAssemblies = overrideAssemblies;
        return this;
    }
}
