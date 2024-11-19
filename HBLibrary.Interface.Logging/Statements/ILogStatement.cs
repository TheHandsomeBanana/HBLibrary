using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HBLibrary.Interface.Logging.Statements;
public interface ILogStatement {
    public string Message { get; set; }
    public string? Name { get; set; }
    public LogLevel? Level { get; set; }
    public DateTime CreatedOn { get; set; }

    public string ToFullString();
    public string ToDefaultString();
}
