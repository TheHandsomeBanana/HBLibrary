using HBLibrary.Interface.Plugins;
using HBLibrary.Interface.Plugins.Provider.Registry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Plugins.Provider.Registry;
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

    public void UnregisterTypes(IAssemblyContext assemblyContext) {
        Type[] types;
        try {
            types = assemblyContext.QueryAll()
                .SelectMany(e => e.GetExportedTypes())
                .ToArray();
        }
        catch (TypeLoadException) {
            // Types are not registered if the assembly version is invalid
            // This occurs if the FileManager.Core.JobSteps Nuget package is not updated for plugins
            // -> Since the application itself also uses the FileManager.Core.JobSteps package. 
            return;
        }

        foreach (Type type in types) {
            registeredTypes.Remove(type.FullName!);
        }
    }
}
