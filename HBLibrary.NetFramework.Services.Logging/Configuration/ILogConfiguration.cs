using HBLibrary.NetFramework.Services.Logging.Targets;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging.Configuration {
    /// <summary>
    /// </summary>
    /// <param name="builder"></param>
    /// <returns><see cref="ILogConfiguration"/> created by <see cref="ILogConfigurationBuilder"/></returns>
    public delegate ILogConfiguration LogConfigurationDelegate(ILogConfigurationBuilder builder);
    public interface ILogConfiguration {
        ImmutableArray<LogTarget> Targets { get; }
        LogDisplayFormat DisplayFormat { get; }
    }

    public enum LogDisplayFormat {
        MessageOnly,
        Minimal,
        Full
    }
}
