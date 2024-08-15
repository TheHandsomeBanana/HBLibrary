namespace HBLibrary.Code.Interpreter.Operation;
public interface IOperation {
    public void Run();
}

public interface IOperation<T> {
    public T Run();
}
