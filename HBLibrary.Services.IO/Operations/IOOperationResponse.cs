using System.Text;

namespace HBLibrary.Services.IO.Operations;
public abstract class IOOperationResponse {
    public bool Success { get; internal set; }
    public string? ErrorMessage { get; internal set; }
    public Exception? Exception { get; internal set; }
    public DateTime ExecutionStart { get; internal set; }
    public DateTime ExecutionEnd { get; internal set; }
    public TimeSpan ExecutionTime => ExecutionEnd - ExecutionStart;

    public virtual string GetStringResult() {
        StringBuilder sb = new StringBuilder();
        sb.Append("Success: " + Success);
        sb.Append($"\nExecution start: {ExecutionStart:HH:mm:ss}\nExecution time: {ExecutionTime:c}");

        if (ErrorMessage is not null)
            sb.Append($"\nError Message: {ErrorMessage}");

        return sb.ToString();
    }
}
