using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Plugins.Provider.Cache;
public interface IPluginTypeCache {
    public PluginType[] Get(IAssemblyContext assemblyContext);
    public IEnumerable<PluginType> QueryAll();

    public IEnumerable<PluginType> QueryByBaseType(Type baseType, IAssemblyContext assemblyContext);
    public IEnumerable<PluginType> QuerySomeByBaseType(Type baseType, IAssemblyContext[] assemblyContext);
    public IEnumerable<PluginType> QueryAllByBaseType(Type baseType);

    public void Add(PluginType type, IAssemblyContext assemblyContext);
    public void AddRange(IEnumerable<PluginType> types, IAssemblyContext assemblyContext);
    public bool Remove(PluginType type, IAssemblyContext assemblyContext);
    public bool RemoveAll(IAssemblyContext assemblyContext);
    public bool ContainsContext(IAssemblyContext assemblyContext);
}
