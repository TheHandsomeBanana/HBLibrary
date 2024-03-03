using HBLibrary.Code.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Code.Interpreter.Syntax;
public interface ISyntaxError {
    public TextSpan FullSpan { get; }
    public string? Content { get; }
    public string? Message { get; }
}
