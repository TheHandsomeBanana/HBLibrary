using HBLibrary.Interface.IO;
using HBLibrary.Interface.IO.Archiving.WinRAR.Commands;
using HBLibrary.Interface.IO.Archiving.WinRAR.Commands.Builder;

namespace HBLibrary.IO.Archiving.WinRAR.Commands.Builder;
internal class WinRARDeleteCommandBuilder : IWinRARDeleteCommandBuilder {
    public string Build() {
        throw new NotImplementedException();
    }

    public IWinRARDeleteCommandBuilder SetPassword(WinRARPassword password) {
        throw new NotImplementedException();
    }

    public IWinRARDeleteCommandBuilder SetProcessPriority(WinRARProcessPriority priority) {
        throw new NotImplementedException();
    }

    public IWinRARDeleteCommandBuilder SetTargetArchive(ValidPath targetArchive) {
        throw new NotImplementedException();
    }
}
