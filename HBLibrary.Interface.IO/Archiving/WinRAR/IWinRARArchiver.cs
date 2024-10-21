using HBLibrary.Core.Process;
using HBLibrary.Interface.IO.Archiving.WinRAR.Commands;

namespace HBLibrary.Interface.IO.Archiving.WinRAR;
/// <summary>
/// RAR is used for creating compressed archives in the RAR format, 
/// while UnRAR is used for extracting files from those archives. 
/// RAR is proprietary and requires a license for use beyond a trial period, 
/// whereas UnRAR is generally free to use for extracting files.
/// </summary>
public interface IWinRARArchiver<TCommand> where TCommand : IWinRARCommand {
    public WinRARCommandExecutionResult Execute(TCommand command, int timeout = Timeout.Infinite);
#if NET5_0_OR_GREATER
    public Task<WinRARCommandExecutionResult> ExecuteAsync(TCommand command, CancellationToken cancellationToken = default);
#endif
}

/// <summary>
/// RAR is used for creating compressed archives in the RAR format, 
/// while UnRAR is used for extracting files from those archives. 
/// RAR is proprietary and requires a license for use beyond a trial period, 
/// whereas UnRAR is generally free to use for extracting files.
/// </summary>
public interface IWinRARArchiver {
    public event EventHandler<ProcessExitEventArgs>? OnProcessExit;
    public event EventHandler<ProcessStdStreamEventArgs>? OnOutputDataReceived;
    public event EventHandler<ProcessStdStreamEventArgs>? OnErrorDataReceived;

    public WinRARCommandExecutionResult Execute(IWinRARCommand command, int timeout = Timeout.Infinite);
#if NET5_0_OR_GREATER
    public Task<WinRARCommandExecutionResult> ExecuteAsync(IWinRARCommand command, CancellationToken cancellationToken = default);
#endif
}