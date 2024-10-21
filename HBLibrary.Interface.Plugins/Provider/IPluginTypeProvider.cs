using HBLibrary.Interface.Plugins.Provider.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Plugins.Provider;
public interface IPluginTypeProvider {
    public IPluginTypeCache TypeCache { get; }

    public PluginType[] GetFromBaseType(Type baseType, IAssemblyContext[] usedContexts);
    public PluginType[] GetFromBaseType<T>(IAssemblyContext[] usedContexts) where T : class;

    public IEnumerable<PluginType> QueryFromBaseType(Type baseType, IAssemblyContext[] usedContexts);
    public IEnumerable<PluginType> QueryFromBaseType<T>(IAssemblyContext[] usedContexts) where T : class;

    public PluginType[] GetByAttribute(IAssemblyContext[] usedContexts);
    public PluginType[] GetByAttribute<T>(IAssemblyContext[] usedContexts) where T : class;

    public IEnumerable<PluginType> QueryByAttribute(IAssemblyContext[] usedContexts);
    public IEnumerable<PluginType> QueryByAttribute<T>(IAssemblyContext[] usedContexts) where T : class;

    public PluginType[] GetCachedFromBaseType(Type baseType, IAssemblyContext[] usedContexts);
    public PluginType[] GetCachedFromBaseType<T>(IAssemblyContext[] usedContexts) where T : class;

    public IEnumerable<PluginType> QueryCachedFromBaseType(Type baseType, IAssemblyContext[] usedContexts);
    public IEnumerable<PluginType> QueryCachedFromBaseType<T>(IAssemblyContext[] usedContexts) where T : class;

    public PluginType[] GetCachedByAttribute(IAssemblyContext[] usedContexts);
    public PluginType[] GetCachedByAttribute<T>(IAssemblyContext[] usedContexts) where T : class;

    public IEnumerable<PluginType> QueryCachedByAttribute(IAssemblyContext[] usedContexts);
    public IEnumerable<PluginType> QueryCachedByAttribute<T>(IAssemblyContext[] usedContexts) where T : class;
}
