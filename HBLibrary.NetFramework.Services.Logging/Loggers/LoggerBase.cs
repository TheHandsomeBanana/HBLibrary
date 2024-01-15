using HBLibrary.NetFramework.Services.Logging.Configuration;
using HBLibrary.NetFramework.Services.Logging.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging.Loggers {
    public abstract class LoggerBase {
        public string Category { get; protected set; }
        public ILogConfiguration Configuration { get; protected set; }
        public ILoggerFactory Factory { get; protected set; }

        protected string GetFormattedString(LogStatement log) {
            switch (Configuration.DisplayFormat) {
                case LogDisplayFormat.Normal:
                    return log.ToString();
                case LogDisplayFormat.Minimal:
                    return log.ToMinimalString();
                case LogDisplayFormat.Full:
                    return log.ToFullString();
                default:
                    throw new NotSupportedException(Configuration.DisplayFormat.ToString());
            }
        }
    }
}
