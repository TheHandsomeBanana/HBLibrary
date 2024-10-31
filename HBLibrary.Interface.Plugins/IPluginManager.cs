using HBLibrary.DataStructures;
using HBLibrary.Interface.Plugins.Configuration;
using HBLibrary.Interface.Plugins.Configuration.Builder;
using HBLibrary.Interface.Plugins.Loader;
using HBLibrary.Interface.Plugins.Provider;
using HBLibrary.Interface.Plugins.Provider.Registry;
using HBLibrary.Interface.Plugins.Provider.Resolver;
using Microsoft.SqlServer.Server;
using System.Collections.Immutable;
using System.Reflection;

namespace HBLibrary.Interface.Plugins;
public interface IPluginManager {
    public bool AssembliesLoaded { get; }
    public IPMConfiguration Configuration { get; }
    public IAssemblyLoader Loader { get; }
    public IPluginTypeProvider TypeProvider { get; }
    public IPluginTypeRegistry TypeRegistry { get; }
    public IPluginTypeResolver TypeResolver { get; }

    public Result AddOrUpdatePluginAssembly(string assemblyFullPath);
    public Result RemovePluginAssembly(string assemblyFileName);
    public bool ContainsPluginAssembly(string assemblyFileName);
    public bool IsPluginAssemblyLoaded(string assemblyFileName);

    public Result RemovePluginAssembly(AssemblyName assemblyName);
    public bool ContainsPluginAssembly(AssemblyName assemblyName);
    public bool IsPluginAssemblyLoaded(AssemblyName assemblyName);

    public Result LoadAssembly(string assemblyFileName);
    public Result LoadAssembly(AssemblyName assemblyName);

    public ImmutableResultCollection LoadAssemblies();
    public IAssemblyContext[] GetLoadedAssemblies();
    public IAssemblyContext? GetLoadedAssembly(string assemblyFileName);
    public IAssemblyContext? GetLoadedAssembly(AssemblyName assemblyName);

    public PluginMetadata GetStaticPluginMetadata(Type stepType);


    /// <summary>
    /// Allows the plugin manager to switch to a different plugin directory
    /// </summary>
    public void SwitchContext(Action<IPMConfigurationBuilder> configBuilder);
}
