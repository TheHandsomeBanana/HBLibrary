using HBLibrary.Common.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Workspace;
public interface IApplicationWorkspaceManager {
    public string Application { get; }
    public ApplicationWorkspace? CurrentWorkspace { get; }
    public Task<bool> WorkspaceExistsAsync(string fullPath);
    public Task<Result<TApplicationWorkspace>> GetAsync<TApplicationWorkspace>(string fullPath) where TApplicationWorkspace : ApplicationWorkspace;
    public Task<Result<TApplicationWorkspace>> OpenAsync<TApplicationWorkspace>(string fullPath, Account.Account executingAccount) where TApplicationWorkspace : ApplicationWorkspace;
    public Task<Result<TApplicationWorkspace>> CreateAndOpenAsync<TApplicationWorkspace>(string fullPath, Account.Account executingAccount) where TApplicationWorkspace : ApplicationWorkspace, new();
    public Task<Result<TApplicationWorkspace>> CreateAndOpenEncryptedAsync<TApplicationWorkspace>(string fullPath, Account.Account executingAccount) where TApplicationWorkspace : ApplicationWorkspace, new();

    public Task<Result> ShareAccess(ApplicationWorkspace workspace, params AccountInfo[] accounts);
    public Task<Result> RevokeAccess(ApplicationWorkspace workspace, params AccountInfo[] accounts);
}

public interface IApplicationWorkspaceManager<TApplicationWorkspace> where TApplicationWorkspace : ApplicationWorkspace, new() {
    public string Application { get; }
    public TApplicationWorkspace? CurrentWorkspace { get; }
    public Task<bool> WorkspaceExistsAsync(string fullPath);
    public Task<Result<TApplicationWorkspace>> GetAsync(string fullPath);
    public Task<Result<TApplicationWorkspace>> OpenAsync(string fullPath, Account.Account executingAccount);
    public Task<Result<TApplicationWorkspace>> CreateAndOpenAsync(string fullPath, Account.Account executingAccount);
    public Task<Result<TApplicationWorkspace>> CreateAndOpenEncryptedAsync(string fullPath, Account.Account executingAccount);

    public Task<Result> ShareAccess(TApplicationWorkspace workspace, params AccountInfo[] accounts);
    public Task<Result> RevokeAccess(TApplicationWorkspace workspace, params AccountInfo[] accounts);
}
