namespace HBLibrary.Code.Interpreter.Operation;
public interface IAsyncOperation {
    public Task Run();
}

public interface IAsyncOperation<T> {
    public Task<T> Run();
}
