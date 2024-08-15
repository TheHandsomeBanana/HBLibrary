namespace HBLibrary.Code.Interpreter.Syntax;
public interface ISyntaxTrivia {
    public bool Leading { get; }
    public TextSpan FullSpan { get; }
}
