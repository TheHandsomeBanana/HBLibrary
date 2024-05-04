using System.Text;

namespace HBLibrary.Services.IO.Obsolete.Operations.File;
public class ReadFileRequest : FileOperationRequest
{
    public override bool CanAsync => true;
    public FileShare Share { get; set; } = FileShare.None;
    public FileAccess Access => FileAccess.Read;
    public Encoding Encoding { get; set; } = Encoding.UTF8;
}