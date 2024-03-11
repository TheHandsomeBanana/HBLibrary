using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands;
public class WinRARUpdateCommand : WinRARCommand {
    public override WinRARCommandName Command => WinRARCommandName.Update;
}
