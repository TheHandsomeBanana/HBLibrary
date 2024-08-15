
/* Unmerged change from project 'HBLibrary.Services.Logging (net472)'
Before:
using System;
After:
using HBLibrary.Services.Logging.Configuration;
using System;
*/

/* Unmerged change from project 'HBLibrary.Services.Logging (net472)'
Before:
using System.Threading.Tasks;
using HBLibrary.Services.Logging.Configuration;
After:
using System.Threading.Tasks;
*/
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


