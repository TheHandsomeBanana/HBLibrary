using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Exceptions;
public class ApplicationWorkspaceException : Exception {
    public ApplicationWorkspaceException(string? message) : base(message) {
    }

    public static ApplicationWorkspaceException AccessDenied(string workspace) {
        return new ApplicationWorkspaceException($"Access to workspace {workspace} denied");
    }

    public static ApplicationWorkspaceException CannotOpen(string reason) {
        return new ApplicationWorkspaceException("Cannot open workspace, " + reason);
    }
    public static ApplicationWorkspaceException CannotGet(string reason) {
        return new ApplicationWorkspaceException("Cannot open workspace, " + reason);
    }
    
    public static ApplicationWorkspaceException NotOpened(string workspace) {
        return new ApplicationWorkspaceException($"{workspace} not opened");
    }

    public static ApplicationWorkspaceException DoesNotExist() {
        return new ApplicationWorkspaceException("Workspace does not exist");
    }
}
