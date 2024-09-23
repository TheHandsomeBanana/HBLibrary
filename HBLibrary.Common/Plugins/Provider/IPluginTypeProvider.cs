using HBLibrary.Common.Plugins.Loader;
using HBLibrary.Common.Plugins.Provider.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Plugins.Provider;
public interface IPluginTypeProvider {
    public IPluginTypeCache TypeCache { get; }

    public PluginType[] GetFromBaseType(Type baseType, AssemblyContext[] usedContexts);
    public PluginType[] GetFromBaseType<T>(AssemblyContext[] usedContexts) where T : class;

    public IEnumerable<PluginType> QueryFromBaseType(Type baseType, AssemblyContext[] usedContexts);
    public IEnumerable<PluginType> QueryFromBaseType<T>(AssemblyContext[] usedContexts) where T : class;

    public PluginType[] GetByAttribute(AssemblyContext[] usedContexts);
    public PluginType[] GetByAttribute<T>(AssemblyContext[] usedContexts) where T : class;

    public IEnumerable<PluginType> QueryByAttribute(AssemblyContext[] usedContexts);
    public IEnumerable<PluginType> QueryByAttribute<T>(AssemblyContext[] usedContexts) where T : class;
    
    public PluginType[] GetCachedFromBaseType(Type baseType, AssemblyContext[] usedContexts);
    public PluginType[] GetCachedFromBaseType<T>(AssemblyContext[] usedContexts) where T : class;

    public IEnumerable<PluginType> QueryCachedFromBaseType(Type baseType, AssemblyContext[] usedContexts);
    public IEnumerable<PluginType> QueryCachedFromBaseType<T>(AssemblyContext[] usedContexts) where T : class;

    public PluginType[] GetCachedByAttribute(AssemblyContext[] usedContexts);
    public PluginType[] GetCachedByAttribute<T>(AssemblyContext[] usedContexts) where T : class;

    public IEnumerable<PluginType> QueryCachedByAttribute(AssemblyContext[] usedContexts);
    public IEnumerable<PluginType> QueryCachedByAttribute<T>(AssemblyContext[] usedContexts) where T : class;
}
