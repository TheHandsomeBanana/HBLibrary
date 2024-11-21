﻿using HBLibrary.Interface.Logging;
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
        if (!IsEnabled) {
            return;
        }

        lock (lockObj) {
            // Concat global targets if registry contains logger
            IEnumerable<ILogTarget> allTargets = Registry != null
                ? Configuration.Targets.Concat(Registry.GlobalConfiguration.Targets)
                : Configuration.Targets;

            ILogStatement log = new LogBlockStatement(block);

            foreach (ILogTarget target in allTargets) {
                target.WriteLog(log, Configuration.Formatter);
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
                    || target.LevelThreshold.HasValue && target.LevelThreshold > level)
                    continue;

                target.WriteLog(log, Configuration.Formatter);
            }
        }
    }

    public void Dispose() {
        Configuration.Dispose();
    }
}