using HBLibrary.Common.Plugins.Configuration;
using HBLibrary.Common.Plugins.Configuration.Builder;
using HBLibrary.Common.Plugins.Loader;
using HBLibrary.Common.Plugins.Provider;
using HBLibrary.Common.Plugins.Provider.Registry;
using HBLibrary.Common.Plugins.Provider.Resolver;
using Microsoft.SqlServer.Server;
using System.Collections.Immutable;
using System.Reflection;

namespace HBLibrary.Common.Plugins;
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
    public AssemblyContext[] GetLoadedAssemblies();
    public AssemblyContext? GetLoadedAssembly(string assemblyFileName);
    public AssemblyContext? GetLoadedAssembly(AssemblyName assemblyName);


    /// <summary>
    /// Allows the plugin manager to switch to a different plugin directory
    /// </summary>
    public void SwitchContext(Action<IPMConfigurationBuilder> configBuilder);
}
