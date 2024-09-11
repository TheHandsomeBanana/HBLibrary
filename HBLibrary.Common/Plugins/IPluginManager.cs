using HBLibrary.Common.Results;

namespace HBLibrary.Common.Plugins;
public interface IPluginManager {
    /// <summary>
    /// Base directory path.
    /// </summary>
    public string BasePath { get; }
    /// <summary>
    /// True after <see cref="LoadAssemblies"/> is called.
    /// </summary>
    public bool AllAssembliesLoaded { get; }
    /// <summary>
    /// Set to true to cache plugin instances.
    /// </summary>
    public bool CachePluginInstances { get; set; }
    /// <summary>
    /// Copies a new assembly to the <see cref="BasePath"/> directory and loads it.
    /// </summary>
    /// <param name="assemblyFullPath"></param>
    public void AddAssembly(string assemblyFullPath);
    /// <summary>
    /// Removes an assembly from the <see cref="BasePath"/> directory.
    /// </summary>
    /// <param name="assemblyFileName"></param>
    public void RemoveAssembly(string assemblyFileName);

    public event Action<Result<string>>? AssemblyLoaded;
    /// <summary>
    /// Loads all the assemblies from the <see cref="BasePath"/> directory.
    /// </summary>
    public void LoadAssemblies();
    /// <summary>
    /// Loads the plugins of the specified type and caches the instances.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void LoadPlugins<T>() where T : class;
    /// <summary>
    /// Loads uncached plugins of the specified type into the cache and and returns all of them.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T[] GetPlugins<T>() where T : class;
    /// <summary>
    /// Loads the instantiable plugin types of the specified type into the cache.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void LoadPluginTypes<T>() where T : class;
    /// <summary>
    /// Loads uncached plugin types of the specified type into the cache and and returns all of them.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Type[] GetPluginTypes<T>() where T : class;
}
