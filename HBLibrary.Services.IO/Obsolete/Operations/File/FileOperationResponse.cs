using System.Text;
using HBLibrary.Services.IO.Obsolete.Operations;

namespace HBLibrary.Services.IO.Obsolete.Operations.File;
public class FileOperationResponse : IOOperationResponse
{
    public FileSnapshot? File { get; internal set; }

    public override string GetStringResult()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(base.ToString());

        if (File.HasValue)
            sb.Append($"\nFile: {File.Value.FullPath}");

        return sb.ToString();
    }
}
