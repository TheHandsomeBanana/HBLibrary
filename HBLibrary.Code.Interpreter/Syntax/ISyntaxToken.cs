using HBLibrary.Code.Interpreter;

namespace HBLibrary.Code.Interpreter.Syntax;
public interface ISyntaxToken {
    public string Value { get; }
    public TextSpan FullSpan { get; }
    public LineSpan LineSpan { get; }
}