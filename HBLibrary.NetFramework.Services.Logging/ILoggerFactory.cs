using HBLibrary.NetFramework.Services.Logging.Configuration;
using HBLibrary.NetFramework.Services.Logging.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HBLibrary.NetFramework.Services.Logging.Exceptions;

namespace HBLibrary.NetFramework.Services.Logging {
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
        /// Creates a <see cref="StandardLogger"/>.<br/>
        /// Not thread safe.
        /// </summary>
        /// <param name="name"></param>
        /// <returns><see cref="StandardLogger"/> instance</returns>
        ILogger CreateStandardLogger(string name);
        /// <summary>
        /// If exists, retrieves <see cref="ILogger"/> from <see cref="Registry"/>.<br/>
        /// If not, creates new <see cref="StandardLogger"/> and adds it to the <see cref="Registry"/>.<br/>
        /// Not thread safe.
        /// </summary>
        /// <param name="name"></param>
        /// <returns><see cref="StandardLogger"/ instance></returns>
        ILogger GetOrCreateStandardLogger(string name);
        /// <summary>
        /// Creates a <see cref="StandardLogger{T}"/>.<br/>
        /// Not thread safe.
        /// </summary>
        /// <param name="name"></param>
        /// <returns><see cref="StandardLogger{T}"/> instance</returns>
        ILogger<T> CreateStandardLogger<T>() where T : class;
        /// <summary>
        /// If exists, retrieves <see cref="ILogger"/> from <see cref="Registry"/>.<br/>
        /// If not, creates new <see cref="StandardLogger{T}"/> and adds it to the <see cref="Registry"/>.<br/>
        /// Not thread safe.
        /// </summary>
        /// <param name="name"></param>
        /// <returns><see cref="StandardLogger{T}"/> instance</returns>
        ILogger<T> GetOrCreateStandardLogger<T>() where T : class;
        /// <summary>
        /// Creates a <see cref="ThreadSafeLogger"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns><see cref="ThreadSafeLogger"/> instance</returns>
        ILogger CreateThreadSafeLogger(string name);
        /// <summary>
        /// If exists, retrieves <see cref="ILogger"/> from <see cref="Registry"/>.<br/>
        /// If not, creates new <see cref="ThreadSafeLogger"/> and adds it to the <see cref="Registry"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns><see cref="ThreadSafeLogger"/> instance</returns>
        ILogger GetOrCreateThreadSafeLogger(string name);
        /// <summary>
        /// Creates a <see cref="ThreadSafeLogger{T}"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns><see cref="ThreadSafeLogger{T}"/> instance</returns>
        ILogger<T> CreateThreadSafeLogger<T>() where T : class;
        /// <summary>
        /// If exists, retrieves <see cref="ILogger"/> from <see cref="Registry"/>.<br/>
        /// If not, creates new <see cref="ThreadSafeLogger{T}"/> and adds it to the <see cref="Registry"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns><see cref="ThreadSafeLogger{T}"/> instance</returns>
        ILogger<T> GetOrCreateThreadSafeLogger<T>() where T : class;
        /// <summary>
        /// Creates an <see cref="AsyncLogger"/>.<br/>
        /// Provides synchronous and asynchronous logging and is thread safe.
        /// </summary>
        /// <param name="name"></param>
        /// <returns><see cref="AsyncLogger"/> instance</returns>
        IAsyncLogger CreateAsyncLogger(string name);
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
        IAsyncLogger<T> CreateAsyncLogger<T>() where T : class;
        /// <summary>
        /// If exists, retrieves <see cref="ILogger"/> from <see cref="Registry"/>.<br/>
        /// If not, creates new <see cref="AsyncLogger{T}"/> and adds it to the <see cref="Registry"/>.<br/>
        /// Provides synchronous and asynchronous logging and is thread safe.
        /// </summary>
        /// <param name="name"></param>
        /// <returns><see cref="AsyncLogger{T}"/> instance</returns>
        IAsyncLogger<T> GetOrCreateAsyncLogger<T>() where T : class;

    }
}
