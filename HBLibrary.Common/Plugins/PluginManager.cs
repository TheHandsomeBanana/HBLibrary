using HBLibrary.Common.Plugins.Configuration;
using HBLibrary.Common.Plugins.Loader;
using HBLibrary.Common.Plugins.Provider;
using HBLibrary.Common.Results;

namespace HBLibrary.Common.Plugins;
public class PluginManager : IPluginManager {
    private readonly Dictionary<string, AssemblyContext> assemblyContexts = [];

    public IPMConfiguration Configuration { get; }
    public IAssemblyLoader Loader { get; }
    public IPluginTypeProvider TypeProvider { get; }
    public bool AssembliesLoaded { get; private set; }

    internal PluginManager(IAssemblyLoader loader, IPluginTypeProvider pluginTypeProvider, IPMConfiguration configuration) {
        this.Configuration = configuration;
        this.Loader = loader;
        this.TypeProvider = pluginTypeProvider;
    }

    public Result AddPluginAssembly(string assemblyFileName) {
        string assemblyPath = Path.Combine(Configuration.Location, assemblyFileName);

        try {
            File.Copy(assemblyFileName, assemblyPath, Configuration.OverrideAssemblies);
        }
        catch (Exception ex) {
            return Result.Fail($"Cannot copy assembly {assemblyFileName} to {Configuration.Location}", ex);
        }

        return LoadAssembly(assemblyPath);
    }

    public Result RemovePluginAssembly(string assemblyFileName) {
        string assemblyPath = Path.Combine(Configuration.Location, assemblyFileName);
        try {
            if (!assemblyContexts.TryGetValue(assemblyPath, out AssemblyContext? assemblyContext)) {
                return Result.Fail($"Assembly {assemblyFileName} is not loaded");
            }

            Loader.Unload(assemblyContext);
            assemblyContexts.Remove(assemblyPath);

            File.Delete(assemblyPath);
            return Result.Ok();
        }
        catch (Exception ex) {
            return Result.Fail($"Failed removing assembly {assemblyFileName} from {Configuration.Location}", ex);
        }
    }

    public CollectionResult LoadAssemblies() {
        if (AssembliesLoaded) {
            return CollectionResult.Ok();
        }

        List<Tuple<string, Exception>> caughtErrors = [];

        foreach (var assembly in Directory.EnumerateFiles(Configuration.Location, "*.dll")) {
            Result res = LoadAssembly(assembly);
            if (res.IsFaulted) {
                caughtErrors.Add(new(res.Message!, res.Exception!));
            }
        }

        if (caughtErrors.Count == 0) {
            AssembliesLoaded = true;
            return CollectionResult.Ok();
        }
        else {
            return CollectionResult.Fail(caughtErrors);
        }
    }

    public Result LoadAssembly(string assemblyFullPath) {
        Result<AssemblyContext> assemblyContext = Loader.LoadAssembly(assemblyFullPath);
        return assemblyContext.Match(e => {
            assemblyContexts.Add(assemblyFullPath, e);
            return Result.Ok();
        }, e => {
            return Result.Fail($"Could not add assembly {assemblyFullPath}", e);
        });
    }

    public void SwitchContext(string basePath) {
        throw new NotImplementedException();
    }
}
