using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands.Builder;
internal class PrebuiltWinRARCommand : IWinRARCommand {
    public WinRARCommandName Command { get; }
    public ImmutableArray<string> Arguments { get; internal set; } = [];
    public ImmutableArray<string> Targets { get; internal set; } = [];

    internal PrebuiltWinRARCommand(WinRARCommandName command) {
        Command = command;
    }

    public string ToCommandString() {
        StringBuilder sb = new StringBuilder();
        sb.Append(WinRARCommand.Get(Command))
            .Append(' ')
            .Append(string.Join(" ", Arguments))
            .Append(' ')
            .Append(string.Join(" ", Targets));

        return sb.ToString();
    }
}
