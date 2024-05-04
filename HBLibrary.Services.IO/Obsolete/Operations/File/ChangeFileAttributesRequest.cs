namespace HBLibrary.Services.IO.Obsolete.Operations.File;
internal class ChangeFileAttributesRequest : FileOperationRequest
{
    public override bool CanAsync => false;
}
