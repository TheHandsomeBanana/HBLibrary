using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Code.Interpreter;
public struct SimpleError {
    public TextSpan Location { get; }
    public LineSpan? LineSpan { get; }
    public string? Affected { get; private set; }
    public string Message { get; }

    public SimpleError(TextSpan location, string? affected, string message) {
        Location = location;
        Affected = affected;
        Message = message;
    }

    public SimpleError(TextSpan location, LineSpan lineSpan, string? affected, string message) : this(location, affected, message) {
        LineSpan = lineSpan;
    }

    public void SetAffected(string affected) {
        Affected = affected;
    }

    public override string ToString() {
        return $"{Location}: {Affected}. Reason: {Message}.";
    }
}