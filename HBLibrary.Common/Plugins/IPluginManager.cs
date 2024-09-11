using HBLibrary.Common.Results;

namespace HBLibrary.Common.Plugins;
public interface IPluginManager {
    public string BasePath { get; set; }
    public void AddAssembly(string assemblyFullPath, bool load);
    public void RemoveAssembly(string assemblyFileName);

    public event Action<Result<string>>? AssemblyLoaded;

    public void LoadAssemblies();
    public void LoadPlugins<T>() where T : class;
    public T[] GetPlugins<T>() where T : class;
}
