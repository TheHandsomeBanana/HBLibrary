using HBLibrary.NetFramework.Services.Logging.Target;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging.Configuration {
    public interface ILogConfiguration {
        ImmutableArray<LogTarget> Targets { get; }
        LogDisplayFormat DisplayFormat { get; }
    }

    public enum LogDisplayFormat {
        Normal,
        Minimal,
        Full
    }
}
