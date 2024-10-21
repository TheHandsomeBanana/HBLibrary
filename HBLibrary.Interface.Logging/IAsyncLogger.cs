namespace HBLibrary.Interface.Logging;
public interface IAsyncLogger : ILogger {
    Task DebugAsync(string message);
    Task InfoAsync(string message);
    Task WarnAsync(string message);
    Task ErrorAsync(string message);
    Task ErrorAsync(Exception exception);
    Task FatalAsync(string message);
}

public interface IAsyncLogger<T> : IAsyncLogger where T : class {
}
