using HBLibrary.Interface.Plugins;
using HBLibrary.Interface.Plugins.Provider.Cache;
using HBLibrary.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Plugins.Provider.Cache;
public class PluginTypeCache : IPluginTypeCache {
    private readonly Dictionary<IAssemblyContext, List<PluginType>> pluginTypes = [];

    public PluginTypeCache() { }

    public void Add(PluginType type, IAssemblyContext assemblyContext) {
        if (pluginTypes.TryGetValue(assemblyContext, out List<PluginType>? value)) {
            value.Add(type);
        }
        else {
            pluginTypes[assemblyContext] = [type];
        }
    }

    public void AddRange(IEnumerable<PluginType> types, IAssemblyContext assemblyContext) {
        if (pluginTypes.TryGetValue(assemblyContext, out List<PluginType>? value)) {
            value.AddRange(types);
        }
        else {
            pluginTypes[assemblyContext] = [.. types];
        }
    }


    public bool Remove(PluginType type, IAssemblyContext assemblyContext) {
        if (pluginTypes.TryGetValue(assemblyContext, out List<PluginType>? value)) {
            bool removed = value.Remove(type);

            if (value.Count == 0) {
                pluginTypes.Remove(assemblyContext);
            }

            return removed;
        }

        return false;
    }

    public bool RemoveAll(IAssemblyContext assemblyContext) {
        return pluginTypes.Remove(assemblyContext);
    }

    public bool ContainsContext(IAssemblyContext assemblyContext) {
        return pluginTypes.ContainsKey(assemblyContext);
    }

    public PluginType[] Get(IAssemblyContext assemblyContext) {
        if (pluginTypes.TryGetValue(assemblyContext, out List<PluginType>? value)) {
            return [.. value];
        }

        return [];
    }

    public IEnumerable<PluginType> QueryAll() {
        return pluginTypes.Values.SelectMany(e => e);
    }

    public IEnumerable<PluginType> QueryByBaseType(Type baseType, IAssemblyContext assemblyContext) {
        if (pluginTypes.TryGetValue(assemblyContext, out List<PluginType>? value)) {
            return value.Where(e => e.BaseType == baseType);
        }

        return [];
    }

    public IEnumerable<PluginType> QuerySomeByBaseType(Type baseType, IAssemblyContext[] assemblyContexts) {
        return pluginTypes.Where(e => assemblyContexts.Any(f => f == e.Key))
            .SelectMany(e => e.Value)
            .Where(e => e.BaseType == baseType);
    }

    public IEnumerable<PluginType> QueryAllByBaseType(Type baseType) {
        return QueryAll().Where(e => e.BaseType == baseType);
    }


}
