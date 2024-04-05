using HBLibrary.Services.Logging.Configuration;
using LightInject;

namespace HBLibrary.Services.Logging.Extensions.LightInject;

public static class ServiceContainerExtensions {
    /// <summary>
    /// Adds <see cref="ILoggerRegistry"/> and <see cref="ILoggerFactory"/> to container.
    /// </summary>
    /// <param name="container"></param>
    /// <returns></returns>
    public static IServiceContainer AddHBLogging(this IServiceContainer container) {
        container.Register<ILoggerRegistry, LoggerRegistry>(new PerContainerLifetime());
        container.Register<ILoggerFactory, LoggerFactory>(new PerContainerLifetime());

        return container;
    }

    /// <summary>
    /// Adds <see cref="ILoggerRegistry"/> based on <paramref name="config"/> and <see cref="ILoggerFactory"/> to container.
    /// </summary>
    /// <param name="container"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IServiceContainer AddHBLogging(this IServiceContainer container, LogConfigurationDelegate config) {
        var registry = LoggerRegistry.FromConfiguration(config);

        container.RegisterInstance<ILoggerRegistry>(registry);
        container.Register<ILoggerFactory, LoggerFactory>(new PerContainerLifetime());

        return container;
    }
}