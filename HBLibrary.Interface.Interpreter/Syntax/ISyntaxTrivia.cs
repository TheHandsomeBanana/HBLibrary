namespace HBLibrary.Interface.Interpreter.Syntax;
public interface ISyntaxTrivia {
    public bool Leading { get; }
    public TextSpan FullSpan { get; }
}
