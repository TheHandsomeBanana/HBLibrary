using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands;
public class WinRARUpdateCommand : WinRARFileEntryCommand {
    public override WinRARCommandName Command => WinRARCommandName.Update; // u
    public WinRARFileNameFormat? FileNameFormat { get; init; } // -cl, -cu
    public WinRARCompressionLevel? CompressionLevel { get; init; } // -m<0..5>
    public DateTime? OnlyUpdateNewerThan { get; init; } // -tn<date/time>
    public DateTime? OnlyUpdateOlderThan { get; init; } // -to<date/time>
    public bool RecurseSubdirectories { get; init; } = true; // -r


    public override string BuildSwitches() {
        StringBuilder sb = new StringBuilder();
        sb.Append(base.BuildSwitches());

        if (FileNameFormat.HasValue)
            sb.Append(WinRARNameMapping.Get(FileNameFormat.Value))
                .Append(' ');

        if (CompressionLevel.HasValue)
            sb.Append(WinRARNameMapping.Get(CompressionLevel.Value))
                .Append(' ');

        if (OnlyUpdateNewerThan.HasValue)
            sb.Append("-tn")
                .Append(OnlyUpdateNewerThan.Value.ToString("yyyyMMddHHmmss"))
                .Append(' ');

        if (OnlyUpdateOlderThan.HasValue)
            sb.Append("-to")
                .Append(OnlyUpdateOlderThan.Value.ToString("yyyyMMddHHmmss"))
                .Append(' ');

        if (RecurseSubdirectories)
            sb.Append("-r ");

        return sb.ToString();
    }

}
