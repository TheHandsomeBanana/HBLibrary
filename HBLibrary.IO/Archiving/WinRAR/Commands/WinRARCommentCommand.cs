﻿using HBLibrary.Interface.IO;
using HBLibrary.Interface.IO.Archiving.WinRAR.Commands;
using System.Text;

namespace HBLibrary.IO.Archiving.WinRAR.Commands;
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
