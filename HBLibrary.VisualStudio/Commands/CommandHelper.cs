using HBLibrary.VisualStudio.Workspace;
using System;

namespace HBLibrary.VisualStudio.Commands;
public static class CommandHelper {
    public static void RunVSCommand(Guid guid, uint id) {
        object pvaln = null;
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
        WorkspaceHelper.GetUIShell().PostExecCommand(guid, id, 0, ref pvaln);
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
    }
}
