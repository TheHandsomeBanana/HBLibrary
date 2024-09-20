using HBLibrary.Common.Plugins.Attributes;
using HBLibrary.Common.Plugins.Configuration;
using HBLibrary.Common.Plugins.Configuration.Builder;
using HBLibrary.Common.Plugins.Loader;
using HBLibrary.Common.Plugins.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Plugins.Builder;
internal class PluginManagerBuilder : IPluginManagerBuilder {
    private IPMConfiguration? configuration;
    private IAssemblyLoader? assemblyLoader;
    private IPluginTypeProvider? typeProvider;

    public IPluginManager Build() {
        if(configuration == null) {
            throw new InvalidOperationException("Configuration is not set.");
        }

        if (assemblyLoader == null) {
            throw new InvalidOperationException("Assembly loader is not set.");
        }

        if (typeProvider == null) {
            throw new InvalidOperationException("Type provider is not set.");
        }

        return new PluginManager(assemblyLoader, typeProvider, configuration);
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
        typeProvider = new PluginTypeProvider();
        return this;
    }

    public IPluginManagerBuilder SetTypeProvider(IPluginTypeProvider typeProvider) {
        this.typeProvider = typeProvider;
        return this;
    }
}
