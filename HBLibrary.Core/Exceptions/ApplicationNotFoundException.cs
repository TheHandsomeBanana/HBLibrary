namespace HBLibrary.Core.Exceptions;
public class ApplicationNotFoundException : Exception {
    public ApplicationNotFoundException(string application) : base($"{application} not found on the system.") { }

    public ApplicationNotFoundException(string application, string message)
        : base($"{application} not found on the system. {message}") { }
}
