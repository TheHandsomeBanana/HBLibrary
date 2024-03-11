using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands;
public class WinRARExtractCommand : WinRARCommand {
    private WinRARCommandName command;
    public override WinRARCommandName Command => command;

    public WinRARExtractCommand(bool extractFull) {
        command = extractFull ? WinRARCommandName.ExtractFull : WinRARCommandName.Extract;
    }
}
