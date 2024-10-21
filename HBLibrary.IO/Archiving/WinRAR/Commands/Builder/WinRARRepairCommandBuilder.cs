using HBLibrary.Interface.IO;
using HBLibrary.Interface.IO.Archiving.WinRAR.Commands;
using HBLibrary.Interface.IO.Archiving.WinRAR.Commands.Builder;

namespace HBLibrary.IO.Archiving.WinRAR.Commands.Builder;
internal class WinRARRepairCommandBuilder : IWinRARRepairCommandBuilder {
    public string Build() {
        throw new NotImplementedException();
    }

    public IWinRARRepairCommandBuilder SetPassword(WinRARPassword password) {
        throw new NotImplementedException();
    }

    public IWinRARRepairCommandBuilder SetProcessPriority(WinRARProcessPriority priority) {
        throw new NotImplementedException();
    }

    public IWinRARRepairCommandBuilder SetTargetArchive(ValidPath targetArchive) {
        throw new NotImplementedException();
    }
}
