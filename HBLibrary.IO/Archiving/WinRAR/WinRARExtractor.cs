using HBLibrary.Interface.IO.Archiving.WinRAR;
using HBLibrary.IO.Archiving.WinRAR.Commands;

namespace HBLibrary.IO.Archiving.WinRAR;
public class WinRARExtractor : IWinRARArchiver<WinRARExtractCommand> {
    private readonly WinRARArchiver archiver = new WinRARArchiver();
    public WinRARCommandExecutionResult Execute(WinRARExtractCommand command, int timeout = -1) {
        return archiver.Execute(command, timeout);
    }

#if NET5_0_OR_GREATER
    public Task<WinRARCommandExecutionResult> ExecuteAsync(WinRARExtractCommand command, CancellationToken cancellationToken = default) {
        return new WinRARArchiver().ExecuteAsync(command, cancellationToken);
    }
#endif
}
