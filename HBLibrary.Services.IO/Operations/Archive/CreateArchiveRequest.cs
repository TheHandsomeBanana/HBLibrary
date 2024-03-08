namespace HBLibrary.Services.IO.Operations.Archive;
public abstract class CreateArchiveRequest : IOOperationRequest {
    public override bool CanAsync => false;
    public required Archiving.Archive Archive { get; set; }
}
