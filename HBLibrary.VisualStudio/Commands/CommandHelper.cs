using HBLibrary.VisualStudio.Workspace;
using System;

namespace HBLibrary.VisualStudio.Commands; 
public static class CommandHelper {
    public static void RunVSCommand(Guid guid, uint id) {
        object pvaln = null;
        WorkspaceHelper.GetUIShell().PostExecCommand(guid, id, 0, ref pvaln);
    }
}
