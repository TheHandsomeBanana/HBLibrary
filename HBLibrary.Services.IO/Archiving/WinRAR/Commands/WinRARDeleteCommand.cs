using System.Text;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands;
public class WinRARDeleteCommand : WinRARFileEntryCommand {
    public override WinRARCommandName Command => WinRARCommandName.Delete;
    public bool RecurseSubdirectories { get; init; } = false; // -r

    public override string BuildSwitches() {
        StringBuilder sb = new StringBuilder();
        sb.Append(base.BuildSwitches());

        if (RecurseSubdirectories)
            sb.Append("-r ");

        return sb.ToString();
    }
}
