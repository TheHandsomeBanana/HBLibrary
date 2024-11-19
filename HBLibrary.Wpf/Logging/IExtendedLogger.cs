using HBLibrary.Interface.Logging;
using HBLibrary.Interface.Logging.Formatting;
using HBLibrary.Interface.Logging.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Logging;
public interface IExtendedLogger : ILogger {
    public void AddBlock(string block);
}
