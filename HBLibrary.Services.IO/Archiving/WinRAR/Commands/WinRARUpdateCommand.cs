using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands;
public class WinRARUpdateCommand : WinRARFileHandlingCommand {
    public override WinRARCommandName Command => WinRARCommandName.Update;
    public WinRARPassword? Password { get; init; } = null;
    public bool RecurseSubdirectories { get; init; } = true;
    public WinRARCompressionLevel CompressionLevel { get; init; } = WinRARCompressionLevel.Normal;
    public DateTime? OnlyUpdateNewerThan { get; init; }
    public DateTime? OnlyUpdateOlderThan { get; init; }
}
