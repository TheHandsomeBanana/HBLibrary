using HBLibrary.Common.Plugins.Attributes;
using HBLibrary.Common.Plugins.Loader;
using HBLibrary.Common.Plugins.Provider.Cache;
using HBLibrary.Common.Plugins.Provider.Registry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Plugins.Provider;
public class PluginTypeProvider : IPluginTypeProvider {
    public IPluginTypeCache TypeCache { get; }
    public PluginTypeProvider(IPluginTypeCache pluginTypeCache) {
        this.TypeCache = pluginTypeCache;
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

    public PluginType[] GetCachedFromBaseType<T>(AssemblyContext[] usedContexts) where T : class {
        return GetCachedFromBaseType(typeof(T), usedContexts);
    }

    public IEnumerable<PluginType> QueryCachedFromBaseType(Type baseType, AssemblyContext[] usedContexts) {
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

    public IEnumerable<PluginType> QueryCachedFromBaseType<T>(AssemblyContext[] usedContexts) where T : class {
        return QueryCachedFromBaseType(typeof(T), usedContexts);
    }

    public PluginType[] GetCachedByAttribute(AssemblyContext[] usedContexts) {
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

    public PluginType[] GetCachedByAttribute<T>(AssemblyContext[] usedContexts) where T : class {
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

    public IEnumerable<PluginType> QueryCachedByAttribute(AssemblyContext[] usedContexts) {
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

    public IEnumerable<PluginType> QueryCachedByAttribute<T>(AssemblyContext[] usedContexts) where T : class {
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
