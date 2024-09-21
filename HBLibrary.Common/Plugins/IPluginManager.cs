using HBLibrary.Common.Plugins.Configuration;
using HBLibrary.Common.Plugins.Configuration.Builder;
using HBLibrary.Common.Plugins.Loader;
using HBLibrary.Common.Plugins.Provider;
using Microsoft.SqlServer.Server;
using System.Collections.Immutable;

namespace HBLibrary.Common.Plugins;
public interface IPluginManager {
    public bool AssembliesLoaded { get; }
    public IPMConfiguration Configuration { get; }
    public IAssemblyLoader Loader { get; }
    public IPluginTypeProvider TypeProvider { get; }

    public Result AddPluginAssembly(string assemblyFullPath);
    public Result RemovePluginAssembly(string assemblyFileName);
    public bool ContainsPluginAssembly(string assemblyFileName);
    public bool IsPluginAssemblyLoaded(string assemblyFileName);

    public ImmutableResultCollection LoadAssemblies();
    public Result LoadAssembly(string assemblyFileName);
    public AssemblyContext[] GetLoadedAssemblies();


    /// <summary>
    /// Allows the plugin manager to switch to a different plugin directory
    /// </summary>
    public void SwitchContext(Action<IPMConfigurationBuilder> configBuilder);
}
