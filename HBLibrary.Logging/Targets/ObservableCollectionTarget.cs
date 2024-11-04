using HBLibrary.Interface.Logging;
using HBLibrary.Interface.Logging.Configuration;
using HBLibrary.Interface.Logging.Statements;
using HBLibrary.Interface.Logging.Targets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Logging.Targets;

public class ObservableCollectionTarget : ILogTarget {
    private readonly ObservableCollection<LogStatement> logs;
    public LogLevel? LevelThreshold { get; }

    public void WriteLog(LogStatement log, LogDisplayFormat displayFormat = LogDisplayFormat.Full) {
        throw new NotImplementedException();
    }

    public void Dispose() {
        throw new NotImplementedException();
    }
}
