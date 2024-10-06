using HBLibrary.Common.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Workspace;
public interface IApplicationWorkspaceManager {
    public string Application { get; }
    public Task<bool> WorkspaceExistsAsync(string fullPath);
    public Task<Result<ApplicationWorkspace>> CreateAsync(string fullPath, Account.Account executingAccount);
    public Task<Result<ApplicationWorkspace>> OpenAsync(string fullPath, Account.Account executingAccount);

    public Task<Result<ApplicationWorkspace>> CreateEncryptedAsync(string fullPath, Account.Account executingAccount);

    public Task<Result> ShareAccess(ApplicationWorkspace workspace, params AccountInfo[] accounts);
    public Task<Result> RevokeAccess(ApplicationWorkspace workspace, params AccountInfo[] accounts);
}
