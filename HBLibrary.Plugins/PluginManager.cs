using HBLibrary.Common.Plugins;
using HBLibrary.DataStructures;
using HBLibrary.Interface.Plugins;
using HBLibrary.Interface.Plugins.Attributes;
using HBLibrary.Interface.Plugins.Builder;
using HBLibrary.Interface.Plugins.Configuration;
using HBLibrary.Interface.Plugins.Configuration.Builder;
using HBLibrary.Interface.Plugins.Loader;
using HBLibrary.Interface.Plugins.Provider;
using HBLibrary.Interface.Plugins.Provider.Registry;
using HBLibrary.Interface.Plugins.Provider.Resolver;
using HBLibrary.Plugins.Builder;
using HBLibrary.Plugins.Configuration;
using HBLibrary.Plugins.Configuration.Builder;
using HBLibrary.Plugins.Loader;
using HBLibrary.Plugins.Provider;
using HBLibrary.Plugins.Provider.Registry;
using HBLibrary.Plugins.Provider.Resolver;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace HBLibrary.Plugins;
public class PluginManager : IPluginManager {
    private readonly Dictionary<string, IAssemblyContext> assemblyContexts = [];

    public IPMConfiguration Configuration { get; private set; }
    public IAssemblyLoader Loader { get; private set; }
    public IPluginTypeProvider TypeProvider { get; private set; }
    public IPluginTypeRegistry TypeRegistry { get; private set; }
    public IPluginTypeResolver TypeResolver { get; private set; }


    public bool AssembliesLoaded { get; private set; } = false;

    public PluginManager(IAssemblyLoader loader,
        IPluginTypeProvider pluginTypeProvider,
        IPMConfiguration configuration,
        IPluginTypeRegistry pluginTypeRegistry,
        IPluginTypeResolver pluginTypeResolver) {

        Configuration = configuration;
        Loader = loader;
        TypeProvider = pluginTypeProvider;
        TypeRegistry = pluginTypeRegistry;
        TypeResolver = pluginTypeResolver;

        if (Configuration.PreloadAssemblies) {
            LoadAssemblies();
        }
    }

    public static IPluginManagerBuilder CreateBuilder() {
        return new PluginManagerBuilder();
    }

    public Result AddOrUpdatePluginAssembly(string assemblyFullPath) {
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
        if (!assemblyFileName.EndsWith(".dll")) {
            assemblyFileName += ".dll";
        }

        string assemblyPath = Path.Combine(Configuration.Location, assemblyFileName);
        try {
            if (!assemblyContexts.TryGetValue(assemblyPath, out IAssemblyContext? assemblyContext)) {
                return Result.Fail($"Assembly {assemblyFileName} is not loaded");
            }

            Loader.Unload(assemblyContext);
            TypeProvider.TypeCache.RemoveAll(assemblyContext);
            TypeRegistry.UnregisterTypes(assemblyContext);
            assemblyContexts.Remove(assemblyPath);

            File.Delete(assemblyPath);

            return Result.Ok();
        }
        catch (Exception ex) {
            return Result.Fail($"Failed removing assembly {assemblyFileName} from {Configuration.Location}", ex);
        }
    }

    public Result RemovePluginAssembly(AssemblyName assemblyName) {
        return RemovePluginAssembly(assemblyName.Name!);
    }

    public bool ContainsPluginAssembly(string assemblyFileName) {
        if (!assemblyFileName.EndsWith(".dll")) {
            assemblyFileName += ".dll";
        }

        string assemblyFullPath = Path.Combine(Configuration.Location, assemblyFileName);
        return Directory.EnumerateFiles(Configuration.Location, "*.dll", SearchOption.AllDirectories)
            .Contains(assemblyFullPath);
    }

    public bool ContainsPluginAssembly(AssemblyName assemblyName) {
        return ContainsPluginAssembly(assemblyName.Name!);
    }

    public bool IsPluginAssemblyLoaded(string assemblyFileName) {
        if (!assemblyFileName.EndsWith(".dll")) {
            assemblyFileName += ".dll";
        }

        return assemblyContexts.ContainsKey(Path.Combine(Configuration.Location, assemblyFileName));
    }

    public bool IsPluginAssemblyLoaded(AssemblyName assemblyName) {
        return IsPluginAssemblyLoaded(assemblyName.Name!);
    }

    public IAssemblyContext? GetLoadedAssembly(string assemblyFileName) {
        if (!assemblyFileName.EndsWith(".dll")) {
            assemblyFileName += ".dll";
        }

        string assemblyFullPath = Path.Combine(Configuration.Location, assemblyFileName);

        if (assemblyContexts.TryGetValue(assemblyFullPath, out IAssemblyContext? assemblyContext)) {
            return assemblyContext;
        }

        return null;
    }

    public IAssemblyContext? GetLoadedAssembly(AssemblyName assemblyName) {
        return GetLoadedAssembly(assemblyName.Name!);
    }

    public ImmutableResultCollection LoadAssemblies() {
        if (AssembliesLoaded) {
            return ResultCollection.Ok();
        }

        ResultCollection results = [];

        foreach (var assembly in Directory.EnumerateFiles(Configuration.Location, "*.dll")) {
            results.Add(LoadAssemblyFromFullPath(assembly));
        }

        if (results.IsSuccess) {
            AssembliesLoaded = true;
        }

        return results;
    }

    public Result LoadAssembly(string assemblyFileName) {
        if (!assemblyFileName.EndsWith(".dll")) {
            assemblyFileName += ".dll";
        }

        string assemblyFullPath = Path.Combine(Configuration.Location, assemblyFileName);
        return LoadAssemblyFromFullPath(assemblyFullPath);
    }

    public Result LoadAssembly(AssemblyName assemblyName) {
        string assemblyFullPath = Path.Combine(Configuration.Location, assemblyName.Name!) + ".dll";
        return LoadAssemblyFromFullPath(assemblyFullPath);
    }

    private Result LoadAssemblyFromFullPath(string assemblyFullPath) {
        if (assemblyContexts.ContainsKey(assemblyFullPath)) {
            return Result.Ok();
        }

        Result<IAssemblyContext> assemblyContext = Loader.LoadAssembly(assemblyFullPath);
        return assemblyContext.Match(e => {
            assemblyContexts.Add(assemblyFullPath, e);
            return Result.Ok();
        }, e => {
            return Result.Fail($"Could not load assembly {assemblyFullPath}", e);
        });
    }

    public void SwitchContext(Action<IPMConfigurationBuilder> configBuilder) {
        foreach (AssemblyContext assemblyContext in assemblyContexts.Values) {
            Loader.Unload(assemblyContext);
            TypeProvider.TypeCache.RemoveAll(assemblyContext);
            TypeRegistry.UnregisterTypes(assemblyContext);
        }

        assemblyContexts.Clear();
        AssembliesLoaded = false;

        PMConfigurationBuilder builder = new PMConfigurationBuilder();
        configBuilder(builder);

        Configuration = builder.Build();

        if (Configuration.PreloadAssemblies) {
            LoadAssemblies();
        }
    }

    public IAssemblyContext[] GetLoadedAssemblies() {
        return [.. assemblyContexts.Values];
    }


    public PluginMetadata GetStaticPluginMetadata(Type stepType) {
        return GetPluginMetadata(stepType);
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
