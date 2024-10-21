namespace HBLibrary.Interface.Interpreter.Exceptions;
public class LexerException : Exception {
    public LexerException(string? message) : base(message) {
    }

    public LexerException(string? message, Exception? innerException) : base(message, innerException) {
    }
}
