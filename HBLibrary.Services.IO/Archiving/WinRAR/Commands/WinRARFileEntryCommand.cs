using System.Text;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands;
public abstract class WinRARFileEntryCommand : WinRARCommand {
    public IEnumerable<string> Targets { get; init; } = [];

    public override string ToCommandString() {
        StringBuilder sb = new StringBuilder();
        sb.Append(base.ToCommandString())
            .Append(' ')
            .Append(string.Join(" ", Targets));

        return sb.ToString();
    }
}
