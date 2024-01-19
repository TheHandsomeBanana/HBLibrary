using HBLibrary.NetFramework.Services.Logging.Configuration;
using HBLibrary.NetFramework.Services.Logging.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging.Targets {
    public interface ILogTarget : IDisposable {
        LogLevel LevelThreshold { get; set; }
        void WriteLog(LogStatement log, LogDisplayFormat displayFormat = LogDisplayFormat.Full);
    }

    public interface IAsyncLogTarget : IDisposable {
        LogLevel LevelThreshold { get; set; }
        Task WriteLogAsync(LogStatement log, LogDisplayFormat displayFormat = LogDisplayFormat.Full);
    }
}
