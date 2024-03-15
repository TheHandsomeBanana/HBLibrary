using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands;
public class WinRARExtractCommand : WinRARFileHandlingCommand {
    private readonly WinRARCommandName command;
    public override WinRARCommandName Command => command;

    public WinRARPassword? Password { get; init; }
    public WinRAROverwriteMode OverwriteMode { get; init; } = WinRAROverwriteMode.Skip;
    public bool RecurseSubdirectories { get; init; } = true;
    public bool KeepBrokenFiles { get; init; } = false;
    public bool AppendArchiveNameToDestination { get; init; } = false;
    public bool IgnoreEmptyDirectories { get; init; } = true;
    

    public WinRARExtractCommand(bool extractFull) {
        command = extractFull ? WinRARCommandName.ExtractFull : WinRARCommandName.Extract;
    }
    
}
