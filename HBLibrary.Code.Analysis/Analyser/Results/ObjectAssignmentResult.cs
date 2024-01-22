using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Code.Analysis.Analyser.Results; 
public enum ObjectAssignmentType {
    Simple,
    Complex
}

public abstract class ObjectAssignmentResult {
    public abstract ObjectAssignmentType Type { get; }
}

public sealed class SimpleObjectAssignmentResult : ObjectAssignmentResult {
    public override ObjectAssignmentType Type => ObjectAssignmentType.Simple;
    public string SimpleValue { get; }
}

public sealed class ComplexObjectAssignmentResult : ObjectAssignmentResult {
    public override ObjectAssignmentType Type => ObjectAssignmentType.Complex;
    public ComplexObjectAssignmentResult ComplexValue { get; }
}
