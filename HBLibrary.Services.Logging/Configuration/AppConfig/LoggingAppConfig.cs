using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HBLibrary.Services.Logging.Configuration;

namespace HBLibrary.Services.Logging.Configuration.AppConfig;

public class LoggingAppConfig {
    public LogLevel GlobalLevel { get; set; }
    public LogDisplayFormat DisplayFormat { get; set; }
    public ConfigLogTarget[] Targets { get; set; } = [];
}

public class ConfigLogTarget {
    public ConfigLogTargetType TargetType { get; set; }
    public LogLevel? TargetLevel { get; set; }
    public string? Filename { get; set; }
    public bool UseAsync { get; set; }
}

public enum ConfigLogTargetType {
    Console,
    Debug,
    File,
}


