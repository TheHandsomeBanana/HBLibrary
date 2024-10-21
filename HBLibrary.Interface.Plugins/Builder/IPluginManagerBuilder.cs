using HBLibrary.Interface.Plugins;
using HBLibrary.Interface.Plugins.Configuration.Builder;
using HBLibrary.Interface.Plugins.Loader;
using HBLibrary.Interface.Plugins.Provider;
using HBLibrary.Interface.Plugins.Provider.Registry;
using HBLibrary.Interface.Plugins.Provider.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Plugins.Builder;
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
