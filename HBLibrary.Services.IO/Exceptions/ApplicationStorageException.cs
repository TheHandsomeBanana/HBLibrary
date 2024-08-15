namespace HBLibrary.Services.IO.Exceptions;
public class ApplicationStorageException : Exception {
    public ApplicationStorageException(string message) : base(message) {
    }

    public ApplicationStorageException(string message, Exception innerException) : base(message, innerException) {
    }
}
