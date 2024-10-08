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
    public Task<Result> OpenAsync<TApplicationWorkspace>(string fullPath, Account.Account executingAccount) where TApplicationWorkspace : ApplicationWorkspace;
    public Task<Result<TApplicationWorkspace>> CreateAsync<TApplicationWorkspace>(string fullPath, Account.Account executingAccount) where TApplicationWorkspace : ApplicationWorkspace, new();
    public Task<Result<TApplicationWorkspace>> CreateEncryptedAsync<TApplicationWorkspace>(string fullPath, Account.Account executingAccount) where TApplicationWorkspace : ApplicationWorkspace, new();

    public Task<Result> ShareAccess(ApplicationWorkspace workspace, params AccountInfo[] accounts);
    public Task<Result> RevokeAccess(ApplicationWorkspace workspace, params AccountInfo[] accounts);
}

public interface IApplicationWorkspaceManager<TApplicationWorkspace> where TApplicationWorkspace : ApplicationWorkspace, new() {
    public string Application { get; }
    public TApplicationWorkspace? CurrentWorkspace { get; }
    public Task<bool> WorkspaceExistsAsync(string fullPath);
    public Task<Result<TApplicationWorkspace>> GetAsync(string fullPath);
    public Task<Result> OpenAsync(string fullPath, Account.Account executingAccount);
    public Task<Result<TApplicationWorkspace>> CreateAsync(string fullPath, Account.Account executingAccount);
    public Task<Result<TApplicationWorkspace>> CreateEncryptedAsync(string fullPath, Account.Account executingAccount);

    public Task<Result> ShareAccess(TApplicationWorkspace workspace, params AccountInfo[] accounts);
    public Task<Result> RevokeAccess(TApplicationWorkspace workspace, params AccountInfo[] accounts);
}
