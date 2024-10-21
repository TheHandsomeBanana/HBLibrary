using HBLibrary.Common.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Plugins.Provider.Resolver;
public interface IPluginTypeResolver {
    public Type? ResolveType(string typeName, IAssemblyContext[] assemblyContexts);
}
