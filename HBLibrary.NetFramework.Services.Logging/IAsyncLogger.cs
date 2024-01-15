using HBLibrary.NetFramework.Services.Logging.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging {
    public interface IAsyncLogger {
        ILogConfiguration Configuration { get; set; }
        void Configure(LogConfigurationDelegate configMethod);
        Task Debug(string message);
        Task Info(string message);
        Task Warn(string message);
        Task Error(string message);
        Task Error(Exception exception);
        Task Fatal(string message);
    }

    public interface IAsyncLogger<T> : IAsyncLogger where T : class {

    }
}
