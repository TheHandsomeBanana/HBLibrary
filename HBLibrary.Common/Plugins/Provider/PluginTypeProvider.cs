using HBLibrary.Common.Plugins.Attributes;
using HBLibrary.Common.Plugins.Loader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Plugins.Provider;
public class PluginTypeProvider : IPluginTypeProvider {
    public PluginTypeProvider() { }

    public PluginType[] GetFromBaseType(Type baseType, AssemblyContext[] usedContexts) {
        return [.. QueryFromBaseType(baseType, usedContexts)];
    }

    public PluginType[] GetFromBaseType<T>(AssemblyContext[] usedContexts) where T : class {
        return [.. QueryFromBaseType<T>(usedContexts)];
    }

    public IEnumerable<PluginType> QueryFromBaseType(Type baseType, AssemblyContext[] usedContexts) {
        return usedContexts
            .SelectMany(e => e.QueryAll().SelectMany(e => e.GetExportedTypes()))
            .Where(e => e.IsClass && !e.IsAbstract && baseType.IsAssignableFrom(e))
            .Select(e => new PluginType {
                BaseType = baseType,
                ConcreteType = e,
                Metadata = new PluginMetadata {
                    TypeName = e.GetCustomAttribute<PluginTypeNameAttribute>()?.TypeName ?? e.FullName!,
                    Description = e.GetCustomAttribute<PluginDescriptionAttribute>()?.Description
                }
            });
    }

    public IEnumerable<PluginType> QueryFromBaseType<T>(AssemblyContext[] usedContexts) where T : class {
        return QueryFromBaseType(typeof(T), usedContexts);
    }

    public PluginType[] GetByAttribute(AssemblyContext[] usedContexts) {
        return [.. QueryByAttribute(usedContexts)];
    }

    public IEnumerable<PluginType> QueryByAttribute(AssemblyContext[] usedContexts) {
        return usedContexts
            .SelectMany(e => e.QueryAll().SelectMany(e => e.GetExportedTypes()))
            .Where(e => e.IsDefined(typeof(PluginAttribute), false))
            .Select(e => {
                PluginAttribute pluginAttribute = e.GetCustomAttribute<PluginAttribute>()!;

                return new PluginType {
                    BaseType = pluginAttribute.BaseType,
                    ConcreteType = e,
                    Metadata = new PluginMetadata {
                        TypeName = e.GetCustomAttribute<PluginTypeNameAttribute>()?.TypeName ?? e.FullName!,
                        Description = e.GetCustomAttribute<PluginDescriptionAttribute>()?.Description
                    }
                };
            });
    }

    public PluginType[] GetByAttribute<T>(AssemblyContext[] usedContexts) where T : class {
        return [.. QueryByAttribute<T>(usedContexts)];
    }

    public IEnumerable<PluginType> QueryByAttribute<T>(AssemblyContext[] usedContexts) where T : class {
        return usedContexts
            .SelectMany(e => e.QueryAll().SelectMany(e => e.GetExportedTypes()))
            .Where(e => e.IsDefined(typeof(PluginAttribute<T>), false))
            .Select(e => {
                PluginAttribute<T> pluginAttribute = e.GetCustomAttribute<PluginAttribute<T>>()!;

                return new PluginType {
                    BaseType = pluginAttribute.BaseType,
                    ConcreteType = e,
                    Metadata = new PluginMetadata {

                        TypeName = e.GetCustomAttribute<PluginTypeNameAttribute>()?.TypeName ?? e.FullName!,
                        Description = e.GetCustomAttribute<PluginDescriptionAttribute>()?.Description
                    }
                };
            });
    }
}
