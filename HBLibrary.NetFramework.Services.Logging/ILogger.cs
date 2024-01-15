using HBLibrary.NetFramework.Services.Logging.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging {
    public interface ILogger {
        ILogConfiguration Configuration { get; }
        ILoggerFactory Factory { get; }
        void Configure(LogConfigurationDelegate configMethod);
        void Debug(string message);
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        void Error(Exception exception);
        void Fatal(string message);
    }

    public interface ILogger<T> : ILogger where T : class {
    }

    public enum LogLevel {
        Debug,
        Info,
        Warning,
        Error,
        Fatal
    }
}
