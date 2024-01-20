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
    /// Provides a global configuration for all registered <see cref="ILogger"/> instances.<br/>
    /// Will dispose automatically on process exit.
    /// </summary>
    public interface ILoggerRegistry : IDisposable {
        /// <summary>
        /// <see langword="true"/> if <see cref="ConfigureRegistry(LogConfigurationDelegate)"/> has been called.
        /// </summary>
        bool IsConfigured { get; }
        /// <summary>
        /// Global configuration that is applied to all <see cref="ILogger"/> instances that are either registered,<br/>
        /// or only created and configured with <see cref="ConfigureLogger(ILogger, LogConfigurationDelegate)"/>.<br/>
        /// The global <see cref="LogLevel"/> threshold will overwrite every child threshold.
        /// </summary>
        ILogConfiguration GlobalConfiguration { get; }
        /// <summary>
        /// Contains all registered <see cref="ILogger"/> instances.
        /// </summary>
        IReadOnlyDictionary<string, ILogger> RegisteredLoggers { get; }
        /// <summary>
        /// Updates the <see cref="GlobalConfiguration"/>.<br/>
        /// Overrides the <see cref="ILogger.Configuration"/> from each registered <see cref="ILogger"/>.<br/>
        /// Can only be called once.
        /// </summary>
        /// <param name="configMethod"></param>
        /// <returns></returns>
        ILoggerRegistry ConfigureRegistry(LogConfigurationDelegate configMethod);
        /// <summary>
        /// Overrides an <see cref="ILogger.Configuration"/> based on <see cref="GlobalConfiguration"/> and <paramref name="configMethod"/>.<br/>
        /// If <paramref name="configMethod"/> is <see langword="null"/>, applies only <see cref="GlobalConfiguration"/>.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configMethod"></param>
        void ConfigureLogger(ILogger logger, LogConfigurationDelegate configMethod);
        /// <summary>
        /// Gets an <see cref="ILogger"/> from <see cref="RegisteredLoggers"/> by <paramref name="name"/>
        /// </summary>
        /// <param name="name"></param>
        /// <returns><see cref="ILogger"/></returns>
        /// <exception cref="LoggingException"/>
        ILogger GetLogger(string name);
        /// <summary>
        /// Registers an <see cref="ILogger"/> with its <see cref="ILogger.Name"/> and instance.
        /// </summary>
        /// <param name="logger"></param>
        /// <exception cref="LoggingException"/> 
        void RegisterLogger(ILogger logger);
        /// <summary>
        /// Gets an <see cref="ILogger{T}"/> from <see cref="RegisteredLoggers"/> by <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns><see cref="ILogger{T}"/></returns>
        /// <exception cref="LoggingException"/>
        ILogger<T> GetLogger<T>() where T : class;
        /// <summary>
        /// Gets an <see cref="IAsyncLogger"/> from <see cref="RegisteredLoggers"/> by <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns><see cref="IAsyncLogger"/></returns>
        /// <exception cref="LoggingException"/>
        IAsyncLogger GetAsyncLogger(string name);
        /// <summary>
        /// Gets an <see cref="IAsyncLogger"/> from <see cref="RegisteredLoggers"/> by <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns><see cref="IAsyncLogger"/></returns>
        /// <exception cref="LoggingException"/>
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
    }
}
