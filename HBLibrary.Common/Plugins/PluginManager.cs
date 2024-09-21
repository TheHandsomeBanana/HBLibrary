using HBLibrary.Common.Plugins.Attributes;
using HBLibrary.Common.Plugins.Builder;
using HBLibrary.Common.Plugins.Configuration;
using HBLibrary.Common.Plugins.Configuration.Builder;
using HBLibrary.Common.Plugins.Loader;
using HBLibrary.Common.Plugins.Provider;
using System.Reflection;

namespace HBLibrary.Common.Plugins;
public class PluginManager : IPluginManager {
    private readonly Dictionary<string, AssemblyContext> assemblyContexts = [];

    public IPMConfiguration Configuration { get; private set; }
    public IAssemblyLoader Loader { get; private set; }
    public IPluginTypeProvider TypeProvider { get; private set; }
    public bool AssembliesLoaded { get; private set; } = false;

    public PluginManager(IAssemblyLoader loader, IPluginTypeProvider pluginTypeProvider, IPMConfiguration configuration) {
        this.Configuration = configuration;
        this.Loader = loader;
        this.TypeProvider = pluginTypeProvider;

        if (Configuration.PreloadAssemblies) {
            LoadAssemblies();
        }
    }

    public static IPluginManagerBuilder CreateBuilder() { 
        return new PluginManagerBuilder(); 
    }

    public Result AddPluginAssembly(string assemblyFullPath) {
        string assemblyFileName = Path.GetFileName(assemblyFullPath);
        string newAssemblyPath = Path.Combine(Configuration.Location, assemblyFileName);

        try {
            File.Copy(assemblyFullPath, newAssemblyPath, Configuration.OverrideAssemblies);
        }
        catch (Exception ex) {
            return Result.Fail($"Cannot copy assembly {assemblyFileName} to {Configuration.Location}", ex);
        }

        return LoadAssembly(newAssemblyPath);
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

    public bool ContainsPluginAssembly(string assemblyFileName) {
        string assemblyFullPath = Path.Combine(Configuration.Location, assemblyFileName);
        return Directory.EnumerateFiles(Configuration.Location, "*.dll", SearchOption.AllDirectories)
            .Contains(assemblyFullPath);
    }

    public bool IsPluginAssemblyLoaded(string assemblyFileName) {
        return assemblyContexts.ContainsKey(Path.Combine(Configuration.Location, assemblyFileName));
    }

    public ImmutableResultCollection LoadAssemblies() {
        if (AssembliesLoaded) {
            return ResultCollection.Ok();
        }

        ResultCollection results = [];

        foreach (var assembly in Directory.EnumerateFiles(Configuration.Location, "*.dll")) {
            results.Add(LoadAssembly(assembly));
        }

        if (results.IsSuccess) {
            AssembliesLoaded = true;
        }

        return results;
    }

    public Result LoadAssembly(string assemblyFullPath) {
        if (assemblyContexts.ContainsKey(assemblyFullPath)) {
            return Result.Ok();
        }

        Result<AssemblyContext> assemblyContext = Loader.LoadAssembly(assemblyFullPath);
        return assemblyContext.Match(e => {
            assemblyContexts.Add(assemblyFullPath, e);
            return Result.Ok();
        }, e => {
            return Result.Fail($"Could not load assembly {assemblyFullPath}", e);
        });
    }

    public void SwitchContext(Action<IPMConfigurationBuilder> configBuilder) {
        assemblyContexts.Clear();
        AssembliesLoaded = false;

        PMConfigurationBuilder builder = new PMConfigurationBuilder();
        configBuilder(builder);

        Configuration = builder.Build();

        if(Configuration.PreloadAssemblies) {
            LoadAssemblies();
        }
    }

    public AssemblyContext[] GetLoadedAssemblies() {
        return [.. assemblyContexts.Values];
    }


    #region Static
    private static readonly Dictionary<Type, PluginMetadata> knownPluginMetadata = [];
    public static PluginMetadata GetPluginMetadata(Type stepType) {
        if (knownPluginMetadata.TryGetValue(stepType, out PluginMetadata? pluginMetadata)) {
            return pluginMetadata;
        }

        PluginTypeNameAttribute? stepTypeAttribute = stepType.GetCustomAttribute<PluginTypeNameAttribute>(false);
        PluginDescriptionAttribute? stepDescriptionAttribute = stepType.GetCustomAttribute<PluginDescriptionAttribute>(false);

        pluginMetadata = new PluginMetadata {
            TypeName = stepTypeAttribute is not null ? stepTypeAttribute.TypeName : stepType.FullName!,
            Description = stepDescriptionAttribute?.Description
        };

        knownPluginMetadata[stepType] = pluginMetadata;
        return pluginMetadata;
    }
    #endregion
}
