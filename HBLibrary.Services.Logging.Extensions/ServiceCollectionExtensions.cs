using HBLibrary.Services.Logging.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.Logging.Extensions;
public static class ServiceCollectionExtensions {
    /// <summary>
    /// Adds <see cref="ILoggerRegistry"/> and <see cref="ILoggerFactory"/> to container.
    /// </summary>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static IServiceCollection AddHBLogging(this IServiceCollection collection) {
        collection.AddSingleton<ILoggerRegistry, LoggerRegistry>()
            .AddSingleton<ILoggerFactory, LoggerFactory>();

        return collection;
    }

    /// <summary>
    /// Adds <see cref="ILoggerRegistry"/> based on <paramref name="config"/> and <see cref="ILoggerFactory"/> to container.
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IServiceCollection AddHBLogging(this IServiceCollection collection, LogConfigurationDelegate config) {
        LoggerRegistry registry = LoggerRegistry.FromConfiguration(config);

        collection.AddSingleton<ILoggerRegistry>(registry)
            .AddSingleton<ILoggerFactory, LoggerFactory>();

        return collection;
    }
}
