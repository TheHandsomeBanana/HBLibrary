using HBLibrary.Common.Plugins.Loader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Plugins.Provider.Resolver;
public interface IPluginTypeResolver {
    public Type? ResolveType(string typeName, AssemblyContext[] assemblyContexts); 
}
