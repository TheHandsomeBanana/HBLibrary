using HBLibrary.Common.Results;
using System.Collections;
using System.Reflection;

namespace HBLibrary.Common.Plugins;
public class PluginManager : IPluginManager {
    private readonly Dictionary<string, Assembly> assemblies = [];
    private readonly Dictionary<string, IList> plugins = [];

    public string BasePath { get; set; }
    public PluginManager(string basePath) {
        BasePath = basePath;
    }

    public event Action<Result<string>>? AssemblyLoaded;

    public void AddAssembly(string assemblyFullPath, bool load) {
        string dllName = Path.GetFileName(assemblyFullPath);
        string destPath = Path.Combine(BasePath, dllName);
        File.Copy(assemblyFullPath, destPath, true);

        if (load) {
            LoadAssembly(destPath);
        }
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
    }

    public T[] GetPlugins<T>() where T : class {
        if (plugins.TryGetValue(typeof(T).FullName!, out IList? pluginInstances)) {
            return pluginInstances.OfType<T>()
                .ToArray();
        }

        List<T> instances = GetInstances<T>();
        plugins.Add(typeof(T).FullName!, instances);

        return [.. instances];
    }

    public void LoadPlugins<T>() where T : class {
        if (plugins.ContainsKey(typeof(T).FullName!)) {
            return;
        }

        List<T> instances = GetInstances<T>();
        plugins.Add(typeof(T).FullName!, instances);
    }

    private List<T> GetInstances<T>() where T : class {
        List<T> instances = [];
        foreach (Type type in assemblies.SelectMany(e => e.Value.GetTypes())) {
            if (type.IsAssignableFrom(typeof(T))) {
                T? instance = Activator.CreateInstance(type) as T;

                if (instance is not null) {
                    instances.Add(instance);
                }
            }
        }

        return instances;
    }

    private void LoadAssembly(string assemblyPath) {
        Result<string> result;
        try {
            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            result = Result<string>.Success(assembly.FullName!);

            assemblies.Add(assemblyPath, assembly);
        }
        catch (Exception e) {
            result = Result<string>.Failure(e);
        }

        AssemblyLoaded?.Invoke(result);
    }
}
