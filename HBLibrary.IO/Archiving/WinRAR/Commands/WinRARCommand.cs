using HBLibrary.Core.Limiter;
using HBLibrary.Core.RegularExpressions;
using HBLibrary.Interface.IO.Archiving.WinRAR.Commands;
using System.Text;

namespace HBLibrary.IO.Archiving.WinRAR.Commands;
public abstract class WinRARCommand : IWinRARCommand {
    public virtual WinRARCommandName Command { get; }
    public required string TargetArchive { get; init; }
    public WinRARProcessPriority? Priority { get; init; }
    public WinRARPassword? Password { get; init; }


    public virtual string ToCommandString() {
        StringBuilder sb = new StringBuilder();
        sb.Append(WinRARNameMapping.Get(Command))
            .Append(' ')
            .Append(BuildSwitches())
            .Append(' ')
            .Append(TargetArchive);

        return sb.ToString();
    }

    public virtual string BuildSwitches() {
        StringBuilder sb = new StringBuilder();
        if (Priority.HasValue)
            sb.Append(Priority.Value.ToString())
                .Append(' ');

        if (Password.HasValue)
            sb.Append(Password.Value.ToString())
                .Append(' ');

        return sb.ToString();
    }
}
