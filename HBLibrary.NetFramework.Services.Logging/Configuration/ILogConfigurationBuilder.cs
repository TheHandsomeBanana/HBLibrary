using HBLibrary.NetFramework.Services.Logging.Statements;
using HBLibrary.NetFramework.Services.Logging.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging.Configuration {
    public delegate void LogStatementDelegate(LogStatement log, LogDisplayFormat format = LogDisplayFormat.Full);
    public delegate Task AsyncLogStatementDelegate(LogStatement log, LogDisplayFormat format = LogDisplayFormat.Full);

    public interface ILogConfigurationBuilder {
        ILogConfigurationBuilder AddTarget(ILogTarget target);
        ILogConfigurationBuilder AddAsyncTarget(IAsyncLogTarget target);
        ILogConfigurationBuilder AddFileTarget(string filePath, bool useAsync, LogLevel? levelThreshold = null);
        ILogConfigurationBuilder AddMethodTarget(LogStatementDelegate method, LogLevel? levelThreshold = null);
        ILogConfigurationBuilder AddAsyncMethodTarget(AsyncLogStatementDelegate method, LogLevel? levelThreshold = null);
        ILogConfigurationBuilder WithDisplayFormat(LogDisplayFormat format);
        /// <summary>
        /// Requires to be called before adding targets to overwrite the target level threshold.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        ILogConfigurationBuilder WithLevelThreshold(LogLevel level);
        ILogConfigurationBuilder OverrideConfig(ILogConfiguration logConfiguration);
        ILogConfiguration Build();
    }

    
}
