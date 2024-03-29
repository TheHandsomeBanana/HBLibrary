﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Code.Interpreter.Evaluator;
public struct SemanticError {
    public TextSpan Location { get; }
    public string Affected { get; }
    public string Message { get; }

    public SemanticError(TextSpan location, string affected, string message) {
        Location = location;
        Affected = affected;
        Message = message;
    }

    public override string ToString() {
        return $"{Location}: {Affected}. Reason: {Message}.";
    }
}
