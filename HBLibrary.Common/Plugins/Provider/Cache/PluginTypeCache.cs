using HBLibrary.Common.Plugins.Loader;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Plugins.Provider.Cache;
public class PluginTypeCache : IPluginTypeCache {
    private readonly Dictionary<AssemblyContext, List<PluginType>> pluginTypes = [];

    public PluginTypeCache() { }

    public void Add(PluginType type, AssemblyContext assemblyContext) {
        if (pluginTypes.TryGetValue(assemblyContext, out List<PluginType>? value)) {
            value.Add(type);
        }
        else {
            pluginTypes[assemblyContext] = [type];
        }
    }

    public void AddRange(IEnumerable<PluginType> types, AssemblyContext assemblyContext) {
        if (pluginTypes.TryGetValue(assemblyContext, out List<PluginType>? value)) {
            value.AddRange(types);
        }
        else {
            pluginTypes[assemblyContext] = [.. types];
        }
    }


    public bool Remove(PluginType type, AssemblyContext assemblyContext) {
        if (pluginTypes.TryGetValue(assemblyContext, out List<PluginType>? value)) {
            bool removed = value.Remove(type);

            if (value.Count == 0) {
                pluginTypes.Remove(assemblyContext);
            }

            return removed;
        }

        return false;
    }

    public bool RemoveAll(AssemblyContext assemblyContext) {
        return pluginTypes.Remove(assemblyContext);
    }

    public bool ContainsContext(AssemblyContext assemblyContext) {
        return pluginTypes.ContainsKey(assemblyContext);
    }

    public PluginType[] Get(AssemblyContext assemblyContext) {
        if (pluginTypes.TryGetValue(assemblyContext, out List<PluginType>? value)) {
            return [.. value];
        }

        return [];
    }

    public IEnumerable<PluginType> QueryAll() {
        return pluginTypes.Values.SelectMany(e => e);
    }

    public IEnumerable<PluginType> QueryByBaseType(Type baseType, AssemblyContext assemblyContext) {
        if (pluginTypes.TryGetValue(assemblyContext, out List<PluginType>? value)) {
            return value.Where(e => e.BaseType == baseType);
        }

        return [];
    }

    public IEnumerable<PluginType> QuerySomeByBaseType(Type baseType, AssemblyContext[] assemblyContexts) {
        return pluginTypes.Where(e => assemblyContexts.Any(f => f == e.Key))
            .SelectMany(e => e.Value)
            .Where(e => e.BaseType == baseType);
    }

    public IEnumerable<PluginType> QueryAllByBaseType(Type baseType) {
        return QueryAll().Where(e => e.BaseType == baseType);
    }


}
