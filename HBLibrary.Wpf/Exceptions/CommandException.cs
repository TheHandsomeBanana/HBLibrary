namespace HBLibrary.Wpf.Exceptions;
public class CommandException : Exception {
    public CommandException(Exception innerException) : base(null, innerException) {
    }
    public CommandException(string message, Exception innerException) : base(message, innerException) {
    }
}
