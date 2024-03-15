using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands;
public class WinRARExtractCommand : WinRARFileHandlingCommand {
    private readonly WinRARCommandName command;
    public override WinRARCommandName Command => command;
    public required DirectorySnapshot DestinationDirectory { get; init; }
    public WinRARPassword? Password { get; init; } // -p, -hp
    public WinRAROverwriteMode? OverwriteMode { get; init; } // -o-, -o+
    public bool RecurseSubdirectories { get; init; } = true; // -r
    public bool KeepBrokenFiles { get; init; } = false; // -kb
    public bool AppendArchiveNameToDestination { get; init; } = false; // -ad
    public bool IgnoreEmptyDirectories { get; init; } = true; // -ed


    public WinRARExtractCommand(bool extractFull) {
        command = extractFull ? WinRARCommandName.ExtractFull : WinRARCommandName.Extract;
    }

    public override string ToCommandString() {
        StringBuilder sb = new StringBuilder();
        sb.Append(base.ToCommandString())
            .Append(' ')
            .Append(DestinationDirectory.FullPath)
            .Append('\\');

        return sb.ToString();
    }

    public override string BuildSwitches() {
        StringBuilder sb = new StringBuilder();
        sb.Append(base.BuildSwitches());

        if (Password.HasValue)
            sb.Append(Password.Value.ToString());

        if (OverwriteMode.HasValue)
            switch (OverwriteMode.Value) {
                case WinRAROverwriteMode.Skip:
                    sb.Append("-o- ");
                    break;
                case WinRAROverwriteMode.Silent:
                    sb.Append("-o+ ");
                    break;
            }

        if (RecurseSubdirectories)
            sb.Append("-r ");

        if (KeepBrokenFiles)
            sb.Append("-kb ");

        if (AppendArchiveNameToDestination)
            sb.Append("-ad ");

        if (IgnoreEmptyDirectories)
            sb.Append("-ed ");
            

        return sb.ToString();
    }
}
