namespace HBLibrary.Interface.Workflows;

public interface IWorkflowContext {
    public T Resolve<T>(string qualifiedName);
}
