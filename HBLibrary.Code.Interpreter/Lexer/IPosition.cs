namespace HBLibrary.Code.Interpreter.Lexer;
public interface IPosition {
    public int Index { get; }
    public char GetValue(string content);
}
