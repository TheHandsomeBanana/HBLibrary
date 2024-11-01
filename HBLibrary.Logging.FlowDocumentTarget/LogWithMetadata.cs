using HBLibrary.Interface.Logging.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Logging.FlowDocumentTarget;
public class LogWithMetadata {
    public LogStatement Log { get; set; }
    public bool IsSuccess { get; set; }
}
