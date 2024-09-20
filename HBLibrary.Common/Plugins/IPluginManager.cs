using HBLibrary.Common.Plugins.Configuration;
using HBLibrary.Common.Plugins.Loader;
using HBLibrary.Common.Plugins.Provider;
using HBLibrary.Common.Results;
using Microsoft.SqlServer.Server;
using System.Collections.Immutable;

namespace HBLibrary.Common.Plugins;
public interface IPluginManager {
    public bool AssembliesLoaded { get; }
    public IPMConfiguration Configuration { get; }
    public IAssemblyLoader Loader { get; }
    public IPluginTypeProvider TypeProvider { get; }

    public Result AddPluginAssembly(string assemblyFileName);
    public Result RemovePluginAssembly(string assemblyFileName);

    public CollectionResult LoadAssemblies();
    public Result LoadAssembly(string assemblyFileName);








    /// <summary>
    /// Allows the plugin manager to switch to a different plugin directory
    /// </summary>
    /// <param name="basePath"></param>
    /// <param name="cachePluginInstances"></param>
    public void SwitchContext(string basePath);
}
