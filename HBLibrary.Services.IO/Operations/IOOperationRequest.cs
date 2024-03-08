namespace HBLibrary.Services.IO.Operations;
public abstract class IOOperationRequest {
    public virtual ValidPath Path { get; set; }
    public abstract bool CanAsync { get; }
}
