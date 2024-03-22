using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands.Builder;
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
