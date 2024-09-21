using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Plugins.Configuration.Builder;
public interface IPMConfigurationBuilder {
    public IPMConfiguration Build();
    public IPMConfigurationBuilder SetPluginsLocation(string location);
    public IPMConfigurationBuilder CachePluginInstances(bool cachePluginInstances);
    public IPMConfigurationBuilder PreloadAssemblies(bool preloadAssemblies);
    public IPMConfigurationBuilder OverrideAssemblies(bool overrideAssemblies);
}
