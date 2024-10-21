using HBLibrary.Interface.IO;
using HBLibrary.Interface.IO.Archiving.WinRAR.Commands;
using HBLibrary.Interface.IO.Archiving.WinRAR.Commands.Builder;

namespace HBLibrary.IO.Archiving.WinRAR.Commands.Builder;
internal class WinRARCommentCommandBuilder : IWinRARCommentCommandBuilder {
    public string Build() {
        throw new NotImplementedException();
    }

    public IWinRARCommentCommandBuilder SetPassword(WinRARPassword password) {
        throw new NotImplementedException();
    }

    public IWinRARCommentCommandBuilder SetProcessPriority(WinRARProcessPriority priority) {
        throw new NotImplementedException();
    }

    public IWinRARCommentCommandBuilder SetTargetArchive(ValidPath targetArchive) {
        throw new NotImplementedException();
    }

    public IWinRARCommentCommandBuilder UseFile(FileSnapshot snapshot) {
        throw new NotImplementedException();
    }
}
