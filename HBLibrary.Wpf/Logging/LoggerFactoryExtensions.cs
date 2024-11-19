using HBLibrary.Interface.Logging;
using HBLibrary.Interface.Logging.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Logging;
public static class LoggerFactoryExtensions {
    public static IExtendedLogger CreateExtendedLogger(this ILoggerFactory factory, string name, LogConfigurationDelegate logConfigurationDelegate) {
        return new ExtendedLogger(name, factory.CreateConfiguration(logConfigurationDelegate));
    }
}
