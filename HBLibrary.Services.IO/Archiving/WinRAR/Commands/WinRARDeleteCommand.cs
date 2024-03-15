using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands;
public class WinRARDeleteCommand : WinRARFileHandlingCommand {
    public override WinRARCommandName Command => WinRARCommandName.Delete;
    public WinRARPassword? Password { get; init; } = null; // -p, -hp
    public bool RecurseSubdirectories { get; init; } = false; // -r

}
