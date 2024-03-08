namespace HBLibrary.Services.IO.Operations.File;
internal class ChangeFileAttributesRequest : FileOperationRequest {
    public override bool CanAsync => false;
}
