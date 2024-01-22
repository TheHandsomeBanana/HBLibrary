using HBLibrary.NetFramework.VisualStudio.Workspace;
using System;

namespace HBLibrary.NetFramework.VisualStudio.Commands {
    public static class CommandHelper {
        public static void RunVSCommand(Guid guid, uint id) {
            object pvaln = null;
            WorkspaceHelper.GetUIShell().PostExecCommand(guid, id, 0, ref pvaln);
        }
    }
}
