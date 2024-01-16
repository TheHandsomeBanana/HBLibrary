using HBLibrary.NetFramework.Services.Logging.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging {
    public interface IAsyncLogger : ILogger {
        Task DebugAsync(string message);
        Task InfoAsync(string message);
        Task WarnAsync(string message);
        Task ErrorAsync(string message);
        Task ErrorAsync(Exception exception);
        Task FatalAsync(string message);
    }

    public interface IAsyncLogger<T> : IAsyncLogger where T : class {
    }
}
