using HBLibrary.Common.Process;
using HBLibrary.Services.IO.Archiving.WinRAR.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR;
public class WinRARCompressor : IWinRARArchiver<WinRARAddCommand> {
    private readonly WinRARArchiver archiver = new WinRARArchiver();
    public WinRARCommandExecutionResult Execute(WinRARAddCommand command, int timeout = Timeout.Infinite) {
        return archiver.Execute(command, timeout);
    }

#if NET5_0_OR_GREATER
    public Task<WinRARCommandExecutionResult> ExecuteAsync(WinRARAddCommand command, CancellationToken cancellationToken = default) {
        return new WinRARArchiver().ExecuteAsync(command, cancellationToken);
    }
#endif
}
