using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.LanguageServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace HBLibrary.VisualStudio.Workspace;
public static class WorkspaceHelper {
    public static DTE GetDTE() {
        ThreadHelper.ThrowIfNotOnUIThread();
        return (DTE)Package.GetGlobalService(typeof(DTE));
    }
    
    public static DTE2 GetDTE2() {
        ThreadHelper.ThrowIfNotOnUIThread();
        return (DTE2)Package.GetGlobalService(typeof(DTE));
    }

    // Deprecated according to https://stackoverflow.com/questions/31194968/how-to-register-my-service-as-a-global-service-or-how-can-i-use-mef-in-my-scenar
    // Use SComponentModel & SVsUIShell 
    //public static IComponentModel GetComponentModel() => (IComponentModel)Package.GetGlobalService(typeof(IComponentModel));

    public static IComponentModel GetComponentModel() => (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));

    public static IVsUIShell GetUIShell() {
        ThreadHelper.ThrowIfNotOnUIThread();
        return (IVsUIShell)Package.GetGlobalService(typeof(SVsUIShell));
    }

    public static VisualStudioWorkspace GetVisualStudioWorkspace() => GetComponentModel().GetService<VisualStudioWorkspace>();

    public static Microsoft.CodeAnalysis.Solution GetCurrentCASolution() => GetVisualStudioWorkspace().CurrentSolution;

}
