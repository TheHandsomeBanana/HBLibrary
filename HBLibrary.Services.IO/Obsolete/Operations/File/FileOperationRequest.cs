using HBLibrary.Services.IO.Obsolete.Operations;

namespace HBLibrary.Services.IO.Obsolete.Operations.File;
public abstract class FileOperationRequest : IOOperationRequest
{
    /// <summary>
    /// Implicitly sets <see cref="FileSnapshot"/> <see cref="File"/>, so a valid file path must be provided.
    /// </summary>
    public override ValidPath Path
    {
        set
        {
            File = (FileSnapshot)value;
            base.Path = value;
        }
    }

    public FileSnapshot File { get; private set; }
}