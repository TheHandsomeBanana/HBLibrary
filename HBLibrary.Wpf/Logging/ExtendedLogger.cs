using HBLibrary.Interface.Logging;
using HBLibrary.Interface.Logging.Configuration;
using HBLibrary.Interface.Logging.Formatting;
using HBLibrary.Interface.Logging.Statements;
using HBLibrary.Interface.Logging.Targets;
using HBLibrary.Logging.Statements;
using HBLibrary.Wpf.Logging.Statements;
using Microsoft.Graph.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace HBLibrary.Wpf.Logging;
public class ExtendedLogger : IExtendedLogger {
    public ILoggerRegistry? Registry { get; set; }
    public bool IsEnabled => Registry?.IsEnabled ?? true;
    public string Name { get; protected set; }
    public ILogConfiguration Configuration { get; set; }

    public ExtendedLogger(string name, ILogConfiguration configuration) {
        Name = name;
        Configuration = configuration;
    }

    public void Debug(string message) {
        LogInternal(message, LogLevel.Debug);
    }

    public void Error(string message) {
        LogInternal(message, LogLevel.Error);
    }

    public void Error(Exception exception) {
        LogInternal(exception.ToString(), LogLevel.Debug);
    }

    public void Fatal(string message) {
        LogInternal(message, LogLevel.Fatal);
    }

    public void Info(string message) {
        LogInternal(message, LogLevel.Info);
    }

    public void Warn(string message) {
        LogInternal(message, LogLevel.Warning);
    }

    public void AddBlock(string block) {
        AddBlock(new LogBlockStatement(block));
    }

    public void AddBlock(LogBlockStatement block) {
        if (!IsEnabled) {
            return;
        }

        lock (lockObj) {
            // Concat global targets if registry contains logger
            IEnumerable<ILogTarget> allTargets = Registry != null
                ? Configuration.Targets.Concat(Registry.GlobalConfiguration.Targets)
            : Configuration.Targets;


            foreach (ILogTarget target in allTargets) {
                target.WriteLog(block, Configuration.Formatter);
            }
        }
    }

    public int IndexedDebug(string message) {
        return LogIndexedInternal(message, LogLevel.Debug);
    }

    public int IndexedInfo(string message) {
        return LogIndexedInternal(message, LogLevel.Info);
    }

    public int IndexedWarn(string message) {
        return LogIndexedInternal(message, LogLevel.Warning);
    }

    public int IndexedError(string message) {
        return LogIndexedInternal(message, LogLevel.Error);
    }

    public int IndexedError(Exception exception) {
        return LogIndexedInternal(exception.ToString(), LogLevel.Error);
    }

    public int IndexedFatal(string message) {
        return LogIndexedInternal(message, LogLevel.Fatal);
    }

    public void RewriteIndexed(int index, string message) {
        lock (lockObj) {
            // Concat global targets if registry contains logger
            IEnumerable<IExtendedLogTarget> allTargets = Registry != null
                ? Configuration.Targets.OfType<IExtendedLogTarget>().Concat(Registry.GlobalConfiguration.Targets).OfType<IExtendedLogTarget>()
                : Configuration.Targets.OfType<IExtendedLogTarget>();

            foreach (IExtendedLogTarget target in allTargets) {
                target.RewriteLog(index, message);
            }
        }
    }


    public void LogStatement(ILogStatement logStatement, ILogFormatter formatter) {
        if (!IsEnabled)
            return;

        lock (lockObj) {
            // set right threshold --> Global layer > logger layer > target layer
            LogLevel? levelThreshold = Registry?.GlobalConfiguration.LevelThreshold ?? Configuration.LevelThreshold;

            // Concat global targets if registry contains logger
            IEnumerable<ILogTarget> allTargets = Registry != null
                ? Configuration.Targets.Concat(Registry.GlobalConfiguration.Targets)
                : Configuration.Targets;

            foreach (ILogTarget target in allTargets) {
                // Check for threshold global or per target, no threshold = always log
                if (levelThreshold.HasValue && levelThreshold > logStatement.Level
                    || target.LevelThreshold.HasValue && target.LevelThreshold > logStatement.Level)
                    continue;

                target.WriteLog(logStatement, formatter);
            }
        }
    }

    private static readonly object lockObj = new();
    protected virtual void LogInternal(string message, LogLevel level) {
        if (!IsEnabled)
            return;

        lock (lockObj) {
            // set right threshold --> Global layer > logger layer > target layer
            LogLevel? levelThreshold = Registry?.GlobalConfiguration.LevelThreshold ?? Configuration.LevelThreshold;

            // Concat global targets if registry contains logger
            IEnumerable<ILogTarget> allTargets = Registry != null
                ? Configuration.Targets.Concat(Registry.GlobalConfiguration.Targets)
                : Configuration.Targets;

            ILogStatement log = new LogStatement(message, Name, level, DateTime.Now);

            foreach (ILogTarget target in allTargets) {
                // Check for threshold global or per target, no threshold = always log
                if (levelThreshold.HasValue && levelThreshold > level
                    || target.LevelThreshold.HasValue && target.LevelThreshold > level) {

                    continue;
                }

                target.WriteLog(log, Configuration.Formatter);
            }
        }
    }

    protected virtual int LogIndexedInternal(string message, LogLevel level) {
        if (!IsEnabled)
            return -1;

        lock (lockObj) {
            // set right threshold --> Global layer > logger layer > target layer
            LogLevel? levelThreshold = Registry?.GlobalConfiguration.LevelThreshold ?? Configuration.LevelThreshold;

            // Concat global targets if registry contains logger
            IEnumerable<IExtendedLogTarget> allTargets = Registry != null
                ? Configuration.Targets.OfType<IExtendedLogTarget>().Concat(Registry.GlobalConfiguration.Targets).OfType<IExtendedLogTarget>()
                : Configuration.Targets.OfType<IExtendedLogTarget>();

            ILogStatement log = new LogStatement(message, Name, level, DateTime.Now);
            int index = -1;
            foreach (IExtendedLogTarget target in allTargets) {
                // Check for threshold global or per target, no threshold = always log
                if (levelThreshold.HasValue && levelThreshold > level
                    || target.LevelThreshold.HasValue && target.LevelThreshold > level)
                    continue;

                // Index will remain same for all targets
                index = target.WriteIndexedLog(log, Configuration.Formatter);
            }

            return index;
        }

    }

    public void Dispose() {
        Configuration.Dispose();
    }
}
