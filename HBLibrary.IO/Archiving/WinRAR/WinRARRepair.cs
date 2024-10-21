using HBLibrary.Interface.IO.Archiving.WinRAR;
using HBLibrary.IO.Archiving.WinRAR.Commands;

namespace HBLibrary.IO.Archiving.WinRAR;
public class WinRARRepair : IWinRARArchiver<WinRARRepairCommand> {
    public WinRARCommandExecutionResult Execute(WinRARRepairCommand command, int timeout = -1) {
        return new WinRARArchiver().Execute(command, timeout);
    }
#if NET5_0_OR_GREATER
    public Task<WinRARCommandExecutionResult> ExecuteAsync(WinRARRepairCommand command, CancellationToken cancellationToken = default) {
        return new WinRARArchiver().ExecuteAsync(command, cancellationToken);
    }
#endif
}
