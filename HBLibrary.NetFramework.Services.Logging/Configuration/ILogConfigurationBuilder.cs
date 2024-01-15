using HBLibrary.NetFramework.Services.Logging.Statements;
using HBLibrary.NetFramework.Services.Logging.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging.Configuration {
    public delegate void LogStatementDelegate(LogStatement log);
    public delegate Task AsyncLogStatementDelegate(LogStatement log);

    public interface ILogConfigurationBuilder {
        ILogConfigurationBuilder AddTarget(string filePath, LogLevel minLevel);
        ILogConfigurationBuilder AddTarget(LogStatementDelegate method, LogLevel minLevel);
        ILogConfigurationBuilder WithDisplayFormat(LogDisplayFormat format);
        /// <summary>
        /// Execution is <see langword="async"/> in <see cref="IAsyncLogger"/> only.
        /// </summary>
        ILogConfigurationBuilder AddTarget(AsyncLogStatementDelegate method, LogLevel minLevel);
        ILogConfigurationBuilder OverrideConfig(ILogConfiguration logConfiguration);
        ILogConfiguration Build();
    }

    
}
