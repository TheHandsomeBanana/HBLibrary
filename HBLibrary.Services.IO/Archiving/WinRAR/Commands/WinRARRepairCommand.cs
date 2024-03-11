using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands;
public class WinRARRepairCommand : WinRARCommand {
    public override WinRARCommandName Command => WinRARCommandName.Repair;
}
