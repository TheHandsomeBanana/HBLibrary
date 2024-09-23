using HBLibrary.Common.Plugins.Attributes;
using HBLibrary.Common.Plugins.Configuration;
using HBLibrary.Common.Plugins.Configuration.Builder;
using HBLibrary.Common.Plugins.Loader;
using HBLibrary.Common.Plugins.Provider;
using HBLibrary.Common.Plugins.Provider.Registry;
using HBLibrary.Common.Plugins.Provider.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Plugins.Builder;
public interface IPluginManagerBuilder {
    public IPluginManagerBuilder Configure(Action<IPMConfigurationBuilder> configurationBuilder);
    public IPluginManagerBuilder SetAssemblyLoader(IAssemblyLoader loader);
    public IPluginManagerBuilder SetDefaultAssemblyLoader();
    public IPluginManagerBuilder SetTypeProvider(IPluginTypeProvider typeProvider);
    public IPluginManagerBuilder SetDefaultTypeProvider();
    public IPluginManagerBuilder SetTypeRegistry(IPluginTypeRegistry registry);
    public IPluginManagerBuilder SetDefaultTypeRegistry();
    public IPluginManagerBuilder SetTypeResolver(IPluginTypeResolver resolver);
    public IPluginManagerBuilder SetDefaultTypeResolver();
    public IPluginManager Build();
}
