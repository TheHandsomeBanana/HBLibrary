using HBLibrary.Common.Plugins;
using HBLibrary.Interface.Plugins;
using HBLibrary.Interface.Plugins.Provider;
using HBLibrary.Interface.Plugins.Provider.Cache;
using HBLibrary.Plugins;
using HBLibrary.Plugins.Attributes;
using HBLibrary.Plugins.Provider.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Plugins.Provider;
public class PluginTypeProvider : IPluginTypeProvider {
    public IPluginTypeCache TypeCache { get; }
    public PluginTypeProvider(IPluginTypeCache pluginTypeCache) {
        TypeCache = pluginTypeCache;
    }

    public PluginType[] GetFromBaseType(Type baseType, IAssemblyContext[] usedContexts) {
        return [.. QueryFromBaseType(baseType, usedContexts)];
    }

    public PluginType[] GetFromBaseType<T>(IAssemblyContext[] usedContexts) where T : class {
        return [.. QueryFromBaseType<T>(usedContexts)];
    }

    public IEnumerable<PluginType> QueryFromBaseType(Type baseType, IAssemblyContext[] usedContexts) {
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

    public IEnumerable<PluginType> QueryFromBaseType<T>(IAssemblyContext[] usedContexts) where T : class {
        return QueryFromBaseType(typeof(T), usedContexts);
    }

    public PluginType[] GetByAttribute(IAssemblyContext[] usedContexts) {
        return [.. QueryByAttribute(usedContexts)];
    }

    public IEnumerable<PluginType> QueryByAttribute(IAssemblyContext[] usedContexts) {
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

    public PluginType[] GetByAttribute<T>(IAssemblyContext[] usedContexts) where T : class {
        return [.. QueryByAttribute<T>(usedContexts)];
    }

    public IEnumerable<PluginType> QueryByAttribute<T>(IAssemblyContext[] usedContexts) where T : class {
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


    public PluginType[] GetCachedFromBaseType(Type baseType, IAssemblyContext[] usedContexts) {
        List<AssemblyContext> uncachedContexts = [];
        List<PluginType> cachedTypes = [];

        foreach (AssemblyContext context in usedContexts) {
            if (!TypeCache.ContainsContext(context)) {
                uncachedContexts.Add(context);
                continue;
            }

            cachedTypes.AddRange(TypeCache.QueryByBaseType(baseType, context));
        }


        foreach (AssemblyContext uncachedContext in uncachedContexts) {
            PluginType[] types = uncachedContext.QueryAll().SelectMany(e => e.GetExportedTypes())
                .Where(e => e.IsClass && !e.IsAbstract && baseType.IsAssignableFrom(e))
                .Select(e => new PluginType {
                    BaseType = baseType,
                    ConcreteType = e,
                    Metadata = new PluginMetadata {
                        TypeName = e.GetCustomAttribute<PluginTypeNameAttribute>()?.TypeName ?? e.FullName!,
                        Description = e.GetCustomAttribute<PluginDescriptionAttribute>()?.Description
                    }
                })
                .ToArray(); // Execute query because of cache AddRange

            TypeCache.AddRange(types, uncachedContext);

            cachedTypes.AddRange(types);
        }

        return [.. cachedTypes];
    }

    public PluginType[] GetCachedFromBaseType<T>(IAssemblyContext[] usedContexts) where T : class {
        return GetCachedFromBaseType(typeof(T), usedContexts);
    }

    public IEnumerable<PluginType> QueryCachedFromBaseType(Type baseType, IAssemblyContext[] usedContexts) {
        List<AssemblyContext> uncachedContexts = [];
        IEnumerable<PluginType> cachedTypes = [];

        foreach (AssemblyContext context in usedContexts) {
            if (!TypeCache.ContainsContext(context)) {
                uncachedContexts.Add(context);
                continue;
            }

            cachedTypes = cachedTypes.Concat(TypeCache.QueryByBaseType(baseType, context));
        }


        foreach (AssemblyContext uncachedContext in uncachedContexts) {
            PluginType[] types = uncachedContext.QueryAll().SelectMany(e => e.GetExportedTypes())
                .Where(e => e.IsClass && !e.IsAbstract && baseType.IsAssignableFrom(e))
                .Select(e => new PluginType {
                    BaseType = baseType,
                    ConcreteType = e,
                    Metadata = new PluginMetadata {
                        TypeName = e.GetCustomAttribute<PluginTypeNameAttribute>()?.TypeName ?? e.FullName!,
                        Description = e.GetCustomAttribute<PluginDescriptionAttribute>()?.Description
                    }
                })
                .ToArray(); // Execute query because of cache AddRange

            TypeCache.AddRange(types, uncachedContext);

            cachedTypes = cachedTypes.Concat(types);
        }

        return cachedTypes;
    }

    public IEnumerable<PluginType> QueryCachedFromBaseType<T>(IAssemblyContext[] usedContexts) where T : class {
        return QueryCachedFromBaseType(typeof(T), usedContexts);
    }

    public PluginType[] GetCachedByAttribute(IAssemblyContext[] usedContexts) {
        List<AssemblyContext> uncachedContexts = [];
        List<PluginType> cachedTypes = [];

        foreach (AssemblyContext context in usedContexts) {
            if (!TypeCache.ContainsContext(context)) {
                uncachedContexts.Add(context);
                continue;
            }

            cachedTypes.AddRange(TypeCache.Get(context));
        }

        foreach (AssemblyContext uncachedContext in uncachedContexts) {
            PluginType[] types = uncachedContext.QueryAll().SelectMany(e => e.GetExportedTypes())
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
                })
                .ToArray();

            TypeCache.AddRange(types, uncachedContext);

            cachedTypes.AddRange(types);
        }

        return [.. cachedTypes];
    }

    public PluginType[] GetCachedByAttribute<T>(IAssemblyContext[] usedContexts) where T : class {
        List<AssemblyContext> uncachedContexts = [];
        List<PluginType> cachedTypes = [];

        foreach (AssemblyContext context in usedContexts) {
            if (!TypeCache.ContainsContext(context)) {
                uncachedContexts.Add(context);
                continue;
            }

            cachedTypes.AddRange(TypeCache.QueryByBaseType(typeof(T), context));
        }

        foreach (AssemblyContext uncachedContext in uncachedContexts) {
            PluginType[] types = uncachedContext.QueryAll().SelectMany(e => e.GetExportedTypes())
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
                })
                .ToArray();

            TypeCache.AddRange(types, uncachedContext);

            cachedTypes.AddRange(types);
        }

        return [.. cachedTypes];
    }

    public IEnumerable<PluginType> QueryCachedByAttribute(IAssemblyContext[] usedContexts) {
        List<AssemblyContext> uncachedContexts = [];
        IEnumerable<PluginType> cachedTypes = [];

        foreach (AssemblyContext context in usedContexts) {
            if (!TypeCache.ContainsContext(context)) {
                uncachedContexts.Add(context);
                continue;
            }

            cachedTypes = cachedTypes.Concat(TypeCache.Get(context));
        }

        foreach (AssemblyContext uncachedContext in uncachedContexts) {
            PluginType[] types = uncachedContext.QueryAll().SelectMany(e => e.GetExportedTypes())
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
                })
                .ToArray();

            TypeCache.AddRange(types, uncachedContext);

            cachedTypes = cachedTypes.Concat(types);
        }

        return cachedTypes;
    }

    public IEnumerable<PluginType> QueryCachedByAttribute<T>(IAssemblyContext[] usedContexts) where T : class {
        List<AssemblyContext> uncachedContexts = [];
        IEnumerable<PluginType> cachedTypes = [];

        foreach (AssemblyContext context in usedContexts) {
            if (!TypeCache.ContainsContext(context)) {
                uncachedContexts.Add(context);
                continue;
            }

            cachedTypes = cachedTypes.Concat(TypeCache.QueryByBaseType(typeof(T), context));
        }

        foreach (AssemblyContext uncachedContext in uncachedContexts) {
            PluginType[] types = uncachedContext.QueryAll().SelectMany(e => e.GetExportedTypes())
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
                })
                .ToArray();

            TypeCache.AddRange(types, uncachedContext);

            cachedTypes = cachedTypes.Concat(types);
        }

        return cachedTypes;
    }
}
