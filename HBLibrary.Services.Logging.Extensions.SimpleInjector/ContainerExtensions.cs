using HBLibrary.Services.Logging.Configuration;
using SimpleInjector;

namespace HBLibrary.Services.Logging.Extensions.SimpleInjector;

public static class SimpleInjectorExtensions {
    /// <summary>
    /// Adds <see cref="ILoggerRegistry"/> and <see cref="ILoggerFactory"/> to container.
    /// </summary>
    /// <param name="container"></param>
    /// <returns></returns>
    public static Container AddHBLogging(this Container container) {
        container.RegisterSingleton<ILoggerRegistry, LoggerRegistry>();
        container.RegisterSingleton<ILoggerFactory, LoggerFactory>();

        return container;
    }

    /// <summary>
    /// Adds <see cref="ILoggerRegistry"/> based on <paramref name="config"/> and <see cref="ILoggerFactory"/> to container.
    /// </summary>
    /// <param name="container"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static Container AddHBLogging(this Container container, LogConfigurationDelegate config) {
        var registry = LoggerRegistry.FromConfiguration(config);

        container.RegisterInstance<ILoggerRegistry>(registry);
        container.RegisterSingleton<ILoggerFactory, LoggerFactory>();

        return container;
    }
}
