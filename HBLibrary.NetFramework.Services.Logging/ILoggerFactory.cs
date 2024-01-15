using HBLibrary.NetFramework.Services.Logging.Configuration;
using HBLibrary.NetFramework.Services.Logging.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging {
    public delegate ILogConfiguration LogConfigurationDelegate(ILogConfigurationBuilder builder);
    public interface ILoggerFactory {
        ILogConfiguration Configuration { get; }
        IDictionary<string, ILogger> RegisteredLoggers { get; }
        IDictionary<string, IAsyncLogger> RegisteredAsyncLoggers {get;}
        ILoggerFactory ConfigureFactory(LogConfigurationDelegate configMethod);
        void ConfigureLogger(ILogger logger, LogConfigurationDelegate configMethod);
        void ConfigureLogger(IAsyncLogger logger, LogConfigurationDelegate configMethod);
        ILogger GetOrCreateStandardLogger(string category);
        ILogger<T> GetOrCreateStandardLogger<T>() where T : class;
        ILogger GetOrCreateThreadSafeLogger(string category);
        ILogger<T> GetOrCreateThreadSafeLogger<T>() where T : class;
        IAsyncLogger GetOrCreateAsyncLogger(string category);
        IAsyncLogger<T> GetOrCreateAsyncLogger<T>() where T : class;


    }
}
