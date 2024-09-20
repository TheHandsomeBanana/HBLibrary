using HBLibrary.Common.Plugins.Loader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Plugins.Provider;
public interface IPluginTypeProvider {
    public PluginType[] GetFromBaseType(Type baseType, AssemblyContext[] usedContexts);
    public PluginType[] GetFromBaseType<T>(AssemblyContext[] usedContexts) where T : class;

    public IEnumerable<PluginType> QueryFromBaseType(Type baseType, AssemblyContext[] usedContexts);
    public IEnumerable<PluginType> QueryFromBaseType<T>(AssemblyContext[] usedContexts) where T : class;

    public PluginType[] GetByAttribute(AssemblyContext[] usedContexts);
    public PluginType[] GetByAttribute<T>(AssemblyContext[] usedContexts) where T : class;

    public IEnumerable<PluginType> QueryByAttribute(AssemblyContext[] usedContexts);
    public IEnumerable<PluginType> QueryByAttribute<T>(AssemblyContext[] usedContexts) where T : class;
}
