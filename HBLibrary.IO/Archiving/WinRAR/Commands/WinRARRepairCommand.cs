using HBLibrary.Interface.IO.Archiving.WinRAR.Commands;

namespace HBLibrary.IO.Archiving.WinRAR.Commands;
public class WinRARRepairCommand : WinRARCommand {
    public override WinRARCommandName Command => WinRARCommandName.Repair;
}
