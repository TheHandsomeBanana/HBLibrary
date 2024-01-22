using HBLibrary.NetFramework.Services.Logging.Configuration;
using HBLibrary.NetFramework.Services.Logging.Targets;
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
        /// Toggled by <see cref="Enable"/> and <see cref="Disable"/>.
        /// </summary>
        bool IsEnabled { get; }
        /// <summary>
        /// <see langword="true"/> if <see cref="ConfigureRegistry(LogConfigurationDelegate)"/> has been called.
        /// </summary>
        bool IsConfigured { get; }
        /// <summary>
        /// Global configuration containig targets that are used by all <see cref="ILogger"/> instances that are either registered.<br/>
        /// The global <see cref="LogLevel"/> threshold will overpower every <see cref="ILogger"/> and <see cref="ILogTarget"/> threshold.
        /// </summary>
        ILogConfiguration GlobalConfiguration { get; }
        /// <summary>
        /// Contains all registered <see cref="ILogger"/> instances.
        /// </summary>
        IReadOnlyDictionary<string, ILogger> RegisteredLoggers { get; }
        /// <summary>
        /// <see cref="ILoggerRegistry"/> is enabled by default. Enables logging if disabled.
        /// </summary>
        void Enable();
        /// <summary>
        /// Disables <see cref="ILoggerRegistry"/> including all <see cref="RegisteredLoggers"/>.
        /// </summary>
        void Disable();
        /// <summary>
        /// Updates the <see cref="GlobalConfiguration"/>.<br/>
        /// Should be invoked on startup and can only be called once.
        /// </summary>
        /// <param name="configMethod"></param>
        /// <returns></returns>
        /// <exception cref="LoggingException"></exception>
        ILoggerRegistry ConfigureRegistry(LogConfigurationDelegate configMethod);
        /// <summary>
        /// Overrides the <paramref name="logger"/> <see cref="ILogger.Configuration"/> based on <paramref name="configMethod"/>.<br/>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configMethod"></param>
        /// <exception cref="LoggingException"></exception>
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
