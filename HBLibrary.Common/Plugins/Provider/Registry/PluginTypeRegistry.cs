using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Plugins.Provider.Registry;
public class PluginTypeRegistry : IPluginTypeRegistry {
    private readonly Dictionary<string, Type> registeredTypes = [];

    public Type? GetType(string typeName) {
        return registeredTypes.TryGetValue(typeName, out Type? type) ? type : null;
    }

    public void RegisterType(Type type) {
        registeredTypes[type.AssemblyQualifiedName!] = type;
    }

    public void RegisterTypes(IEnumerable<Type> types) {
        foreach (Type type in types) {
            registeredTypes[type.AssemblyQualifiedName!] = type;
        }
    }

    public bool UnregisterType(string typeName) {
        return registeredTypes.Remove(typeName);
    }

    public void UnregisterTypes(AssemblyContext assemblyContext) {
        foreach(Type type in assemblyContext.QueryAll().SelectMany(e => e.GetExportedTypes())) {
            registeredTypes.Remove(type.FullName!);
        }
    }
}
