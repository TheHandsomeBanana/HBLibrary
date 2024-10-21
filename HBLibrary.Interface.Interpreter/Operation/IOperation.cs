namespace HBLibrary.Interface.Interpreter.Operation;
public interface IOperation {
    public void Run();
}

public interface IOperation<T> {
    public T Run();
}
