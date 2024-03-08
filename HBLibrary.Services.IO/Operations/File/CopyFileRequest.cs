namespace HBLibrary.Services.IO.Operations.File;
public class CopyFileRequest : FileOperationRequest {
    public override bool CanAsync => true;
    public required string TargetFile { get; set; }
    public required CopyConflictAction ConflictAction { get; set; }
    public required IFileEntryService FileEntryService { get; set; }

}
