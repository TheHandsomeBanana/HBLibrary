using HBLibrary.VisualStudio.Workspace;
using Microsoft;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using System;
using System.Threading.Tasks;

namespace HBLibrary.VisualStudio.UI; 
public static class UIHelper {
    public static IServiceProvider Package { get; set; }

    public static void Show(System.Windows.Window window) {
        IVsUIShell uiShell = WorkspaceHelper.GetUIShell();
        Assumes.Present(uiShell);

#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
        uiShell.GetDialogOwnerHwnd(out IntPtr hwnd);
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
        WindowHelper.ShowModal(window, hwnd);
    }

    public static void ShowInfo(string message, string title) {
        VsShellUtilities.ShowMessageBox(
            Package,
            message,
            title,
            OLEMSGICON.OLEMSGICON_INFO,
            OLEMSGBUTTON.OLEMSGBUTTON_OK,
            OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
    }

    public static void ShowInfo(string message) {
        VsShellUtilities.ShowMessageBox(
            Package,
            message,
            null,
            OLEMSGICON.OLEMSGICON_INFO,
            OLEMSGBUTTON.OLEMSGBUTTON_OK,
            OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
    }

    public static void ShowWarning(string message, string title) {
        VsShellUtilities.ShowMessageBox(
            Package,
            message,
            title,
            OLEMSGICON.OLEMSGICON_WARNING,
            OLEMSGBUTTON.OLEMSGBUTTON_OK,
            OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
    }

    public static void ShowWarning(string message) {
        VsShellUtilities.ShowMessageBox(
            Package,
            message,
            null,
            OLEMSGICON.OLEMSGICON_WARNING,
            OLEMSGBUTTON.OLEMSGBUTTON_OK,
            OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
    }

    public static void ShowError(string message, string title) {
        VsShellUtilities.ShowMessageBox(
           Package,
           message,
           title,
           OLEMSGICON.OLEMSGICON_CRITICAL,
           OLEMSGBUTTON.OLEMSGBUTTON_OK,
           OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
    }

    public static void ShowError(string message) {
        VsShellUtilities.ShowMessageBox(
           Package,
           message,
           null,
           OLEMSGICON.OLEMSGICON_CRITICAL,
           OLEMSGBUTTON.OLEMSGBUTTON_OK,
           OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
    }

    public static int ShowWarningWithCancel(string message, string title) {
        return VsShellUtilities.ShowMessageBox(
            Package,
            message,
            title,
            OLEMSGICON.OLEMSGICON_WARNING,
            OLEMSGBUTTON.OLEMSGBUTTON_OKCANCEL,
            OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
    }

    public static int ShowWarningWithCancel(string message) {
        return VsShellUtilities.ShowMessageBox(
            Package,
            message,
            null,
            OLEMSGICON.OLEMSGICON_WARNING,
            OLEMSGBUTTON.OLEMSGBUTTON_OKCANCEL,
            OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
    }

    public static IVsOutputWindowPane CreateOutputWindowPane(string paneName) {
        ThreadHelper.ThrowIfNotOnUIThread();

        IVsOutputWindow logWindow = AsyncPackage.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;
        Guid guid = Guid.NewGuid();
        logWindow.CreatePane(ref guid, paneName, 1, 1);
        logWindow.GetPane(ref guid, out IVsOutputWindowPane pane);
        return pane;
    }
}
