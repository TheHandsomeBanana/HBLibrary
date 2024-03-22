using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands.Builder;
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
