using Autofac;
using HBLibrary.Services.Logging.Configuration;

namespace HBLibrary.Services.Logging.Extensions.Autofac;

public static class ContainerBuilderExtensions {
    /// <summary>
    /// Adds <see cref="ILoggerRegistry"/> and <see cref="ILoggerFactory"/> to container.
    /// </summary>
    /// <param name="containerBuilder"></param>
    /// <returns></returns>
    public static ContainerBuilder AddHBLogging(this ContainerBuilder containerBuilder) {
        containerBuilder.RegisterType<LoggerRegistry>().As<ILoggerRegistry>().SingleInstance();
        containerBuilder.RegisterType<LoggerFactory>().As<ILoggerFactory>().SingleInstance();

        return containerBuilder;
    }

    /// <summary>
    /// Adds <see cref="ILoggerRegistry"/> based on <paramref name="config"/> and <see cref="ILoggerFactory"/> to container.
    /// </summary>
    /// <param name="containerBuilder"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static ContainerBuilder AddHBLogging(this ContainerBuilder containerBuilder, LogConfigurationDelegate config) {
        LoggerRegistry registry = LoggerRegistry.FromConfiguration(config);

        containerBuilder.RegisterInstance(registry).As<ILoggerRegistry>();
        containerBuilder.RegisterType<LoggerFactory>().As<ILoggerFactory>();

        return containerBuilder;
    }
}
