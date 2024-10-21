using HBLibrary.Common.Plugins;
using HBLibrary.Interface.Plugins;
using HBLibrary.Interface.Plugins.Builder;
using HBLibrary.Interface.Plugins.Configuration;
using HBLibrary.Interface.Plugins.Configuration.Builder;
using HBLibrary.Interface.Plugins.Loader;
using HBLibrary.Interface.Plugins.Provider;
using HBLibrary.Interface.Plugins.Provider.Registry;
using HBLibrary.Interface.Plugins.Provider.Resolver;
using HBLibrary.Plugins;
using HBLibrary.Plugins.Configuration;
using HBLibrary.Plugins.Configuration.Builder;
using HBLibrary.Plugins.Loader;
using HBLibrary.Plugins.Provider;
using HBLibrary.Plugins.Provider.Cache;
using HBLibrary.Plugins.Provider.Registry;
using HBLibrary.Plugins.Provider.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Plugins.Builder;
internal class PluginManagerBuilder : IPluginManagerBuilder {
    private IPMConfiguration? configuration;
    private IAssemblyLoader? assemblyLoader;
    private IPluginTypeProvider? typeProvider;
    private IPluginTypeResolver? typeResolver;
    private IPluginTypeRegistry? typeRegistry;

    public IPluginManager Build() {
        if (configuration == null) {
            throw new InvalidOperationException("Configuration is not set.");
        }

        if (assemblyLoader == null) {
            throw new InvalidOperationException("Assembly loader is not set.");
        }

        if (typeProvider == null) {
            throw new InvalidOperationException("Type provider is not set.");
        }

        if (typeResolver == null) {
            throw new InvalidOperationException("Type resolver is not set.");
        }

        if (typeRegistry == null) {
            throw new InvalidOperationException("Type type registry is not set.");
        }

        return new PluginManager(assemblyLoader, typeProvider, configuration, typeRegistry, typeResolver);
    }

    public IPluginManagerBuilder Configure(Action<IPMConfigurationBuilder> configurationBuilder) {
        PMConfigurationBuilder builder = new PMConfigurationBuilder();
        configurationBuilder(builder);
        configuration = builder.Build();
        return this;
    }

    public IPluginManagerBuilder SetAssemblyLoader(IAssemblyLoader loader) {
        assemblyLoader = loader;
        return this;
    }

    public IPluginManagerBuilder SetDefaultAssemblyLoader() {
        assemblyLoader = new AssemblyLoader();
        return this;
    }

    public IPluginManagerBuilder SetDefaultTypeProvider() {
        typeProvider = new PluginTypeProvider(new PluginTypeCache());
        return this;
    }

    public IPluginManagerBuilder SetTypeProvider(IPluginTypeProvider typeProvider) {
        this.typeProvider = typeProvider;
        return this;
    }

    public IPluginManagerBuilder SetDefaultTypeRegistry() {
        typeRegistry = new PluginTypeRegistry();
        return this;
    }

    public IPluginManagerBuilder SetDefaultTypeResolver() {
        typeResolver = new PluginTypeResolver();
        return this;
    }

    public IPluginManagerBuilder SetTypeRegistry(IPluginTypeRegistry registry) {
        typeRegistry = registry;
        return this;
    }

    public IPluginManagerBuilder SetTypeResolver(IPluginTypeResolver resolver) {
        typeResolver = resolver;
        return this;
    }
}
