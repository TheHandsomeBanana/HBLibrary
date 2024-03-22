using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands.Builder;
internal class PrebuiltWinRARCommand : IWinRARCommand {
    public WinRARCommandName Command { get; }
    public string ArgumentString { get; }

    internal PrebuiltWinRARCommand(WinRARCommandName command, string argumentString) {
        Command = command;
        ArgumentString = argumentString;
    }

    public string ToCommandString() {
        StringBuilder sb = new StringBuilder();
        sb.Append(WinRARNameMapping.Get(Command))
            .Append(' ')
            .Append(ArgumentString);

        return sb.ToString();
    }
}
