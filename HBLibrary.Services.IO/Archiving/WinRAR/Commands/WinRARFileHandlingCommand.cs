using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands;
public abstract class WinRARFileHandlingCommand : WinRARCommand {
    public IEnumerable<string> Targets { get; init; } = [];

    public override string ToCommandString() {
        StringBuilder sb = new StringBuilder();
        sb.Append(base.ToCommandString())
            .Append(' ')
            .Append(BuildContextArguments())
            .Append(' ')
            .Append(string.Join(' ', Targets));

        return sb.ToString();
    }

    public abstract string BuildContextArguments();
}
