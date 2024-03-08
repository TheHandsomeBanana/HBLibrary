namespace HBLibrary.Services.IO.Operations.File;
public abstract class FileOperationRequest : IOOperationRequest {
    /// <summary>
    /// Implicitly sets <see cref="FileSnapshot"/> <see cref="File"/>, so a valid file path must be provided.
    /// </summary>
    public override ValidPath Path {
        set {
            File = (FileSnapshot)value;
            base.Path = value;
        }
    }

    public FileSnapshot File { get; private set; }
}