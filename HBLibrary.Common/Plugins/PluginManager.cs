using HBLibrary.Common.Results;
using System.Collections;
using System.Reflection;

namespace HBLibrary.Common.Plugins;
public class PluginManager : IPluginManager {
    private readonly Dictionary<string, Assembly> assemblies = [];
    private readonly Dictionary<string, IList> plugins = [];
    private readonly Dictionary<Type, Type[]> pluginTypes = [];

    // Keeps track of added assemblies that are not loaded during runtime.
    // Ignored for initial load
    private readonly List<string> toLoadAssemblies = [];

    public string BasePath { get; }
    public bool AllAssembliesLoaded { get; private set; } = false;
    public bool CachePluginInstances { get; set; } = false;

    public PluginManager(string basePath) {
        BasePath = basePath;
        Directory.CreateDirectory(BasePath);
    }

    public event Action<Result<string>>? AssemblyLoaded;

    public void AddAssembly(string assemblyFullPath) {
        string dllName = Path.GetFileName(assemblyFullPath);
        string destPath = Path.Combine(BasePath, dllName);
        File.Copy(assemblyFullPath, destPath, true);
        LoadAssembly(destPath);
    }

    public void RemoveAssembly(string assemblyFileName) {
        assemblies.Remove(assemblyFileName);
        File.Delete(Path.Combine(BasePath, assemblyFileName));
    }

    public void LoadAssemblies() {
        IEnumerable<string> assembliesToLoad = Directory.EnumerateFiles(BasePath, "*.dll", SearchOption.AllDirectories)
            .Where(e => !assemblies.Any(a => a.Key == e));

        foreach (string file in assembliesToLoad) {
            LoadAssembly(file);
        }

        AllAssembliesLoaded = true;
    }

    public T[] GetPlugins<T>() where T : class {
        if (CachePluginInstances) {
            if (plugins.TryGetValue(typeof(T).FullName!, out IList? pluginInstances)) {
                return pluginInstances.OfType<T>()
                    .ToArray();
            }

            List<T> instances = GetInstances<T>();
            plugins.Add(typeof(T).FullName!, instances);

            return [.. instances];
        }
        
        return [.. GetInstances<T>()];
    }

    public void LoadPlugins<T>() where T : class {
        if (!CachePluginInstances) {
            return;
        }

        if (plugins.ContainsKey(typeof(T).FullName!)) {
            return;
        }

        plugins.Add(typeof(T).FullName!, GetInstances<T>());
    }

    private List<T> GetInstances<T>() where T : class {
        List<T> instances = [];
        if (!pluginTypes.ContainsKey(typeof(T))) {
            LoadPluginTypes<T>();
        }

        Type[] types = pluginTypes[typeof(T)];

        foreach (Type type in types) {
            T? instance = Activator.CreateInstance(type) as T;

            if (instance is not null) {
                instances.Add(instance);
            }
        }

        return instances;
    }

    private void LoadAssembly(string assemblyPath) {
        Result<string> result;
        try {
            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            result = Result<string>.Ok(assembly.FullName!);

            assemblies.Add(assemblyPath, assembly);
        }
        catch (Exception e) {
            result = Result<string>.Fail(e);
        }

        AssemblyLoaded?.Invoke(result);
    }

    public void LoadPluginTypes<T>() where T : class {
        if (this.pluginTypes.ContainsKey(typeof(T))) {
            return;
        }

        Type[] types = GetPluginTypesInternal<T>();
        this.pluginTypes.Add(typeof(T), types);
    }

    public Type[] GetPluginTypes<T>() where T : class {
        if (this.pluginTypes.TryGetValue(typeof(T), out Type[]? pluginTypes)) {
            return pluginTypes;
        }

        Type[] types = GetPluginTypesInternal<T>();

        this.pluginTypes.Add(typeof(T), types);
        return types;
    }

    private Type[] GetPluginTypesInternal<T>() where T : class {
        List<Type> result = [];
        foreach (Assembly assembly in assemblies.Values) {
            foreach (Type type in assembly.GetTypes()) {
                if (type.IsAssignableFrom(typeof(T))) {
                    result.Add(type);
                }
            }
        }

        return [.. result];
    }
}
