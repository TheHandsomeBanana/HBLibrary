using HBLibrary.Common.Plugins.Loader;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Plugins.Provider.Cache;
public interface IPluginTypeCache {
    public PluginType[] Get(AssemblyContext assemblyContext);
    public IEnumerable<PluginType> QueryAll();

    public IEnumerable<PluginType> QueryByBaseType(Type baseType, AssemblyContext assemblyContext);
    public IEnumerable<PluginType> QuerySomeByBaseType(Type baseType, AssemblyContext[] assemblyContext);
    public IEnumerable<PluginType> QueryAllByBaseType(Type baseType);

    public void Add(PluginType type, AssemblyContext assemblyContext);
    public void AddRange(IEnumerable<PluginType> types, AssemblyContext assemblyContext);
    public bool Remove(PluginType type, AssemblyContext assemblyContext);
    public bool RemoveAll(AssemblyContext assemblyContext);
    public bool ContainsContext(AssemblyContext assemblyContext); 
}
