using HBLibrary.Services.Logging.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace HBLibrary.Services.Logging.Extensions;
public static class UnityContainerExtensions {
    /// <summary>
    /// Adds <see cref="ILoggerRegistry"/> and <see cref="ILoggerFactory"/> to container.
    /// </summary>
    /// <param name="container"></param>
    /// <returns></returns>
    public static IUnityContainer AddHBLogging(this IUnityContainer container) {
        container.RegisterSingleton<ILoggerRegistry, LoggerRegistry>()
            .RegisterSingleton<ILoggerFactory, LoggerFactory>();

        return container;
    }

    /// <summary>
    /// Adds <see cref="ILoggerRegistry"/> based on <paramref name="config"/> and <see cref="ILoggerFactory"/> to container.
    /// </summary>
    /// <param name="container"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IUnityContainer AddHBLogging(this IUnityContainer container, LogConfigurationDelegate config) {
        LoggerRegistry registry = LoggerRegistry.FromConfiguration(config);

        container.RegisterInstance<ILoggerRegistry>(registry, InstanceLifetime.Singleton)
            .RegisterSingleton<ILoggerFactory, LoggerFactory>();

        return container;
    }
}
