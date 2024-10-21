namespace HBLibrary.IO.Exceptions;
public class ApplicationStorageException : Exception {
    public ApplicationStorageException(string message) : base(message) {
    }

    public ApplicationStorageException(string message, Exception innerException) : base(message, innerException) {
    }
}
