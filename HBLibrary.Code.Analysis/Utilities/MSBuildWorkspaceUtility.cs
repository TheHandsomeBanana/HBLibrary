using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
using System.Reflection;
using System.Text;

namespace HBLibrary.Code.Analysis.Utilities;
public static class MSBuildWorkspaceUtility {
    public static MSBuildWorkspace OpenWorkspace() {
        if (!MSBuildLocator.IsRegistered)
            MSBuildLocator.RegisterDefaults();

        return MSBuildWorkspace.Create();
    }

    public static (string, ReflectionTypeLoadException) HandleReflectionTypeLoad(ReflectionTypeLoadException ex) {
        StringBuilder sb = new StringBuilder();
        foreach (Exception? exSub in ex.LoaderExceptions) {
            if (exSub == null)
                continue;

            sb.AppendLine(exSub.Message);

            if (exSub is FileNotFoundException exFileNotFound) {
                if (!string.IsNullOrEmpty(exFileNotFound.FusionLog)) {
                    sb.AppendLine("Fusion Log:");
                    sb.AppendLine(exFileNotFound.FusionLog);
                }
            }
            sb.AppendLine();
        }

        return (sb.ToString(), ex);
    }
}
