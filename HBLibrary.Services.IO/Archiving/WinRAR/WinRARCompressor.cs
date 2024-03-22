using HBLibrary.Common.Process;
using HBLibrary.Services.IO.Archiving.WinRAR.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR;
public class WinRARCompressor : IWinRARArchiver<WinRARAddCommand> {
    public WinRARCommandExecutionResult Execute(WinRARAddCommand command, int timeout = Timeout.Infinite) {
        return new WinRARArchiver().Execute(command, timeout);
    }

#if NET5_0_OR_GREATER
    public Task<WinRARCommandExecutionResult> ExecuteAsync(WinRARAddCommand command, CancellationToken cancellationToken = default) {
        return new WinRARArchiver().ExecuteAsync(command, cancellationToken);
    }
#endif
}
