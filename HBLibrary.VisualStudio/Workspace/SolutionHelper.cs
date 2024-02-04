using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System.Collections.Generic;
using System.Linq;

namespace HBLibrary.VisualStudio.Workspace; 
public static class SolutionHelper {
    public static IEnumerable<Project> GetProjects(Solution solution) {
        ThreadHelper.ThrowIfNotOnUIThread();
        foreach (Project project in solution.Projects)
            yield return project;
    }

    public static Project GetSelectedProject() {
        DTE dte = WorkspaceHelper.GetDTE();

#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
        if ((dte.SelectedItems.Item(1).Project == null && dte.SelectedItems.Item(1).ProjectItem?.ContainingProject == null) || dte.SelectedItems.Count == 0)
            return dte.ActiveDocument?.ProjectItem?.ContainingProject;
        else
            return (dte.SelectedItems.Item(1).Project == null) ? dte.SelectedItems.Item(1).ProjectItem.ContainingProject : dte.SelectedItems.Item(1).Project;
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
    }

    public static Microsoft.CodeAnalysis.Project ToCAProject(Project project) {
        ThreadHelper.ThrowIfNotOnUIThread();

#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
        return WorkspaceHelper.GetCurrentCASolution().Projects.FirstOrDefault(e => e.FilePath == project.FullName);
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
    }

    public static Microsoft.CodeAnalysis.Project GetCurrentCAProject() => ToCAProject(GetSelectedProject());
}
