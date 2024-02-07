using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Operations;
public abstract class FileOperationResponse {
    public FileSnapshot? File { get; init; }
    public bool Success { get; init; }
    public string? Message { get; init; }
    public FileContentType? ContentType { get; init; } = null;
    public DateTime ExecutionStart { get; init; }
    public DateTime ExecutionEnd { get; init; }
    public TimeSpan Duration => ExecutionEnd - ExecutionStart;

    public abstract string GetStringResult();

    public override string ToString() {
        StringBuilder sb = new StringBuilder();
        sb.Append(Success.ToString());
        sb.Append($"; Execution start: {ExecutionStart}, Duration: {Duration}");
        
        if (File.HasValue)
            sb.Append($"; File: {File.Value.FullPath}");

        if(Message != null)
            sb.Append("; " + Message);

        if(ContentType.HasValue)
            sb.Append("; " + ContentType);

        return sb.ToString();
    }
}
