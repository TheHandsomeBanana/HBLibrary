
using HBLibrary.Interface.Logging.Configuration;

namespace HBLibrary.Interface.Logging;
/// <summary>
/// Provides thread safe and asynchronous logging.<br/>
/// Uses <see cref="ILoggerRegistry"/> as container.
/// </summary>
public interface ILoggerFactory {
    /// <summary>
    /// Contains instances of <see cref="ILogger"/> and configurations.
    /// </summary>
    ILoggerRegistry Registry { get; }
    /// <summary>
    /// Creates a <see cref="Logger"/>.<br/>
    /// </summary>
    /// <param name="name"></param>
    /// <returns><see cref="Logger"/> instance</returns>
    ILogger CreateLogger(string name, LogConfigurationDelegate configuration);
    /// <summary>
    /// If exists, retrieves <see cref="ILogger"/> from <see cref="Registry"/>.<br/>
    /// If not, creates new <see cref="Logger"/> and adds it to the <see cref="Registry"/>.<br/>
    /// </summary>
    /// <param name="name"></param>
    /// <returns><see cref="Logger"/ instance></returns>
    ILogger GetOrCreateLogger(string name);
    /// <summary>
    /// Creates a <see cref="Logger{T}"/>.<br/>
    /// </summary>
    /// <param name="name"></param>
    /// <returns><see cref="Logger{T}"/> instance</returns>
    ILogger<T> CreateLogger<T>(LogConfigurationDelegate configuration) where T : class;
    /// <summary>
    /// If exists, retrieves <see cref="ILogger"/> from <see cref="Registry"/>.<br/>
    /// If not, creates new <see cref="Logger{T}"/> and adds it to the <see cref="Registry"/>.<br/>
    /// </summary>
    /// <param name="name"></param>
    /// <returns><see cref="Logger{T}"/> instance</returns>
    ILogger<T> GetOrCreateLogger<T>() where T : class;
    /// <summary>
    /// Creates an <see cref="AsyncLogger"/>.<br/>
    /// Provides synchronous and asynchronous logging and is thread safe.
    /// </summary>
    /// <param name="name"></param>
    /// <returns><see cref="AsyncLogger"/> instance</returns>
    IAsyncLogger CreateAsyncLogger(string name, LogConfigurationDelegate configuration);
    /// <summary>
    /// If exists, retrieves <see cref="ILogger"/> from <see cref="Registry"/>.<br/>
    /// If not, creates new <see cref="AsyncLogger"/> and adds it to the <see cref="Registry"/>.<br/>
    /// Provides synchronous and asynchronous logging and is thread safe.
    /// </summary>
    /// <param name="name"></param>
    /// <returns><see cref="AsyncLogger"/> instance</returns>
    IAsyncLogger GetOrCreateAsyncLogger(string name);
    /// <summary>
    /// Creates an <see cref="AsyncLogger{T}"/>.<br/>
    /// Provides synchronous and asynchronous logging and is thread safe.
    /// </summary>
    /// <param name="name"></param>
    /// <returns><see cref="AsyncLogger{T}"/> instance</returns>
    IAsyncLogger<T> CreateAsyncLogger<T>(LogConfigurationDelegate configuration) where T : class;
    /// <summary>
    /// If exists, retrieves <see cref="ILogger"/> from <see cref="Registry"/>.<br/>
    /// If not, creates new <see cref="AsyncLogger{T}"/> and adds it to the <see cref="Registry"/>.<br/>
    /// Provides synchronous and asynchronous logging and is thread safe.
    /// </summary>
    /// <param name="name"></param>
    /// <returns><see cref="AsyncLogger{T}"/> instance</returns>
    IAsyncLogger<T> GetOrCreateAsyncLogger<T>() where T : class;

}
