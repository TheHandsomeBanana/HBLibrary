namespace HBLibrary.Interface.Interpreter.Operation;
public interface IAsyncOperation {
    public Task Run();
}

public interface IAsyncOperation<T> {
    public Task<T> Run();
}
