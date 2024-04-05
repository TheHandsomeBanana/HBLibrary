using HBLibrary.Services.Logging.Configuration;
using Ninject;

namespace HBLibrary.Services.Logging.Extensions.Ninject;

public static class KernelExtensions {
    /// <summary>
    /// Adds <see cref="ILoggerRegistry"/> and <see cref="ILoggerFactory"/> to kernel.
    /// </summary>
    /// <param name="kernel"></param>
    /// <returns></returns>
    public static IKernel AddHBLogging(this IKernel kernel) {
        kernel.Bind<ILoggerRegistry>().To<LoggerRegistry>().InSingletonScope();
        kernel.Bind<ILoggerFactory>().To<LoggerFactory>().InSingletonScope();

        return kernel;
    }

    /// <summary>
    /// Adds <see cref="ILoggerRegistry"/> based on <paramref name="config"/> and <see cref="ILoggerFactory"/> to kernel.
    /// </summary>
    /// <param name="kernel"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IKernel AddHBLogging(this IKernel kernel, LogConfigurationDelegate config) {
        var registry = LoggerRegistry.FromConfiguration(config);

        kernel.Bind<ILoggerRegistry>().ToConstant(registry); // Ninject manages singleton scope for constant bindings by default
        kernel.Bind<ILoggerFactory>().To<LoggerFactory>().InSingletonScope();

        return kernel;
    }
}
