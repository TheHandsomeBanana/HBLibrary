using HBLibrary.NetFramework.Services.Logging.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HBLibrary.NetFramework.Services.Logging.Exceptions;

namespace HBLibrary.NetFramework.Services.Logging {
    /// <summary>
    /// Container for <see cref="ILogger"/> instances and their <see cref="ILogConfiguration"/>.<br/>
    /// Provides a global configuration for all registered <see cref="ILogger"/> instances.
    /// </summary>
    public interface ILoggerRegistry {
        /// <summary>
        /// Global configuration that is applied to all <see cref="ILogger"/> instances that are either registered,<br/>
        /// or only created and configured with <see cref="ConfigureLogger(ILogger, LogConfigurationDelegate)"/> 
        /// </summary>
        ILogConfiguration GlobalConfiguration { get; }
        /// <summary>
        /// Contains all registered <see cref="ILogger"/> instances.
        /// </summary>
        IReadOnlyDictionary<string, ILogger> RegisteredLoggers { get; }
        /// <summary>
        /// Contains <see cref="ILogConfiguration"/> instances from every registered or configured <see cref="ILogger"/>.
        /// </summary>
        IReadOnlyDictionary<string, ILogConfiguration> LoggerConfigurations { get; }
        /// <summary>
        /// Updates the <see cref="GlobalConfiguration"/>.<br/>
        /// Overrides the <see cref="ILogger.Configuration"/> from each registered <see cref="ILogger"/>.
        /// </summary>
        /// <param name="configMethod"></param>
        /// <returns></returns>
        ILoggerRegistry ConfigureRegistry(LogConfigurationDelegate configMethod);
        /// <summary>
        /// Updates an <see cref="ILogger.Configuration"/>.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configMethod"></param>
        void ConfigureLogger(ILogger logger, LogConfigurationDelegate configMethod);
        /// <summary>
        /// Gets an <see cref="ILogger"/> from <see cref="RegisteredLoggers"/> by <paramref name="name"/>
        /// </summary>
        /// <param name="name"></param>
        /// <returns><see cref="ILogger"/></returns>
        /// <exception cref="LoggerException"/>
        ILogger GetLogger(string name);
        /// <summary>
        /// Registers an <see cref="ILogger"/> with its <see cref="ILogger.Name"/> and instance.
        /// </summary>
        /// <param name="logger"></param>
        /// <exception cref="LoggerException"/> 
        void RegisterLogger(ILogger logger);
        /// <summary>
        /// Gets an <see cref="ILogger{T}"/> from <see cref="RegisteredLoggers"/> by <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns><see cref="ILogger{T}"/></returns>
        /// <exception cref="LoggerException"/>
        ILogger<T> GetLogger<T>() where T : class;
        /// <summary>
        /// Gets an <see cref="IAsyncLogger"/> from <see cref="RegisteredLoggers"/> by <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns><see cref="IAsyncLogger"/></returns>
        /// <exception cref="LoggerException"/>
        IAsyncLogger GetAsyncLogger(string name);
        /// <summary>
        /// Gets an <see cref="IAsyncLogger"/> from <see cref="RegisteredLoggers"/> by <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns><see cref="IAsyncLogger"/></returns>
        /// <exception cref="LoggerException"/>
        IAsyncLogger<T> GetAsyncLogger<T>() where T : class;
        /// <summary>
        /// Checks wether an <see cref="ILogger"/> is registered by <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns><see cref="bool"/></returns>
        bool ContainsLogger(string name);
        /// <summary>
        /// Checks wether an <see cref="ILogger{T}"/> is registered.
        /// </summary>
        /// <param name="name"></param>
        /// <returns><see cref="bool"/></returns>
        bool ContainsLogger<T>() where T : class;
        /// <summary>
        /// Gets an <see cref="ILogConfiguration"/> from <see cref="LoggerConfigurations"/> by <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ILogConfiguration GetConfiguration(string name);
    }
}
