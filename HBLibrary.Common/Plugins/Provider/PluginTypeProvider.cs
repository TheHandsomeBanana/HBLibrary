using HBLibrary.Common.Plugins.Attributes;
using HBLibrary.Common.Plugins.Loader;
using HBLibrary.Common.Plugins.Provider.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Plugins.Provider;
public class PluginTypeProvider : IPluginTypeProvider {
    private readonly IPluginTypeCache pluginTypeCache;
    public PluginTypeProvider(IPluginTypeCache pluginTypeCache) {
        this.pluginTypeCache = pluginTypeCache;
    }

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


    public PluginType[] GetCachedFromBaseType(Type baseType, AssemblyContext[] usedContexts) {
        List<AssemblyContext> uncachedContexts = [];
        List<PluginType> cachedTypes = [];

        foreach (AssemblyContext context in usedContexts) {
            if (!pluginTypeCache.ContainsContext(context)) {
                uncachedContexts.Add(context);
                continue;
            }

            cachedTypes.AddRange(pluginTypeCache.QueryByBaseType(baseType, context));
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

            pluginTypeCache.AddRange(types, uncachedContext);

            cachedTypes.AddRange(types);
        }

        return [.. cachedTypes];
    }

    public PluginType[] GetCachedFromBaseType<T>(AssemblyContext[] usedContexts) where T : class {
        return GetCachedFromBaseType(typeof(T), usedContexts);
    }

    public IEnumerable<PluginType> QueryCachedFromBaseType(Type baseType, AssemblyContext[] usedContexts) {
        List<AssemblyContext> uncachedContexts = [];
        IEnumerable<PluginType> cachedTypes = [];

        foreach (AssemblyContext context in usedContexts) {
            if (!pluginTypeCache.ContainsContext(context)) {
                uncachedContexts.Add(context);
                continue;
            }

            cachedTypes = cachedTypes.Concat(pluginTypeCache.QueryByBaseType(baseType, context));
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

            pluginTypeCache.AddRange(types, uncachedContext);

            cachedTypes = cachedTypes.Concat(types);
        }

        return cachedTypes;
    }

    public IEnumerable<PluginType> QueryCachedFromBaseType<T>(AssemblyContext[] usedContexts) where T : class {
        return QueryCachedFromBaseType(typeof(T), usedContexts);
    }

    public PluginType[] GetCachedByAttribute(AssemblyContext[] usedContexts) {
        List<AssemblyContext> uncachedContexts = [];
        List<PluginType> cachedTypes = [];

        foreach (AssemblyContext context in usedContexts) {
            if (!pluginTypeCache.ContainsContext(context)) {
                uncachedContexts.Add(context);
                continue;
            }

            cachedTypes.AddRange(pluginTypeCache.Get(context));
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

            pluginTypeCache.AddRange(types, uncachedContext);

            cachedTypes.AddRange(types);
        }

        return [.. cachedTypes];
    }

    public PluginType[] GetCachedByAttribute<T>(AssemblyContext[] usedContexts) where T : class {
        List<AssemblyContext> uncachedContexts = [];
        List<PluginType> cachedTypes = [];

        foreach (AssemblyContext context in usedContexts) {
            if (!pluginTypeCache.ContainsContext(context)) {
                uncachedContexts.Add(context);
                continue;
            }

            cachedTypes.AddRange(pluginTypeCache.QueryByBaseType(typeof(T), context));
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

            pluginTypeCache.AddRange(types, uncachedContext);

            cachedTypes.AddRange(types);
        }

        return [.. cachedTypes];
    }

    public IEnumerable<PluginType> QueryCachedByAttribute(AssemblyContext[] usedContexts) {
        List<AssemblyContext> uncachedContexts = [];
        IEnumerable<PluginType> cachedTypes = [];

        foreach (AssemblyContext context in usedContexts) {
            if (!pluginTypeCache.ContainsContext(context)) {
                uncachedContexts.Add(context);
                continue;
            }

            cachedTypes = cachedTypes.Concat(pluginTypeCache.Get(context));
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

            pluginTypeCache.AddRange(types, uncachedContext);

            cachedTypes = cachedTypes.Concat(types);
        }

        return cachedTypes;
    }

    public IEnumerable<PluginType> QueryCachedByAttribute<T>(AssemblyContext[] usedContexts) where T : class {
        List<AssemblyContext> uncachedContexts = [];
        IEnumerable<PluginType> cachedTypes = [];

        foreach (AssemblyContext context in usedContexts) {
            if (!pluginTypeCache.ContainsContext(context)) {
                uncachedContexts.Add(context);
                continue;
            }

            cachedTypes = cachedTypes.Concat(pluginTypeCache.QueryByBaseType(typeof(T), context));
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

            pluginTypeCache.AddRange(types, uncachedContext);

            cachedTypes = cachedTypes.Concat(types);
        }

        return cachedTypes;
    }
}
