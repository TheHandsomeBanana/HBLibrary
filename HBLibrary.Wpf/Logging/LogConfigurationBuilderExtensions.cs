using HBLibrary.Interface.Logging;
using HBLibrary.Interface.Logging.Configuration;
using HBLibrary.Interface.Logging.Formatting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Logging;
public static class LogConfigurationBuilderExtensions {
    public static ILogConfigurationBuilder AddListBoxLogTarget(this ILogConfigurationBuilder builder, LogLevel? minLevel = null, ILogFormatter? formatter = null) {
        builder.AddTarget(new ListBoxLogTarget(minLevel, formatter));
        return builder;
    }
}
