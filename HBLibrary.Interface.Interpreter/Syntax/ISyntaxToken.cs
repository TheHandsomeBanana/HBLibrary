namespace HBLibrary.Interface.Interpreter.Syntax;
public interface ISyntaxToken {
    public string Value { get; }
    public TextSpan FullSpan { get; }
    public LineSpan LineSpan { get; }
}