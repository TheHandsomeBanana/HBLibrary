using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands;
public class WinRARCommentCommand : WinRARCommand {
    public override WinRARCommandName Command => WinRARCommandName.Comment;
    public required FileSnapshot File { get; init; }

    public override string ToCommandString() {
        StringBuilder sb = new StringBuilder();
        sb.Append(base.ToCommandString())
            .Append(' ')
            .Append("-z")
            .Append(File.FullPath)
            .Append(' ')
            .Append(TargetArchive);

        return sb.ToString();
    }
}
