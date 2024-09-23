using HBLibrary.Common.Plugins.Loader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Plugins.Provider.Resolver;
public class PluginTypeResolver : IPluginTypeResolver {
    public Type? ResolveType(string typeName, AssemblyContext[] assemblyContexts) {
        Type? type = Type.GetType(typeName);
        if (type is not null) {
            return type;
        }

        foreach (AssemblyContext assemblyContext in assemblyContexts) {
            foreach (Assembly assembly in assemblyContext.QueryAll()) {
                type = assembly.GetType(typeName, false);
                if (type is not null) {
                    return type;
                }
            }
        }

        return null;
    }
}
