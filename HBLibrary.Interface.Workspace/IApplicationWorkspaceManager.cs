using HBLibrary.DataStructures;
using HBLibrary.Interface.Security.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Workspace;
public interface IApplicationWorkspaceManager {
    public string Application { get; }
    public IApplicationWorkspace? CurrentWorkspace { get; }
    public event Action? OnWorkspaceOpened;
    public Task<bool> WorkspaceExistsAsync(string fullPath);
    public Task<Result<TApplicationWorkspace>> GetAsync<TApplicationWorkspace>(string fullPath, IAccount executingAccount) where TApplicationWorkspace : IApplicationWorkspace;
    public Task<Result> OpenAsync<TApplicationWorkspace>(string fullPath, IAccount executingAccount) where TApplicationWorkspace : IApplicationWorkspace;
    public Task<Result<TApplicationWorkspace>> CreateAsync<TApplicationWorkspace>(string fullPath, IAccount executingAccount) where TApplicationWorkspace : IApplicationWorkspace, new();
    public Task<Result<TApplicationWorkspace>> CreateEncryptedAsync<TApplicationWorkspace>(string fullPath, IAccount executingAccount) where TApplicationWorkspace : IApplicationWorkspace, new();

    public bool IsOwner(IApplicationWorkspace workspace, IAccount executingAccount);
    public Result ShareAccess(IApplicationWorkspace workspace, params IAccountInfo[] accounts);
    public Result RevokeAccess(IApplicationWorkspace workspace, params IAccountInfo[] accounts);
}

public interface IApplicationWorkspaceManager<TApplicationWorkspace> where TApplicationWorkspace : class, IApplicationWorkspace, new() {
    public string Application { get; }
    public TApplicationWorkspace? CurrentWorkspace { get; }
    public event Action? OnWorkspaceOpened;

    public Task<bool> WorkspaceExistsAsync(string fullPath);
    public Task<Result<TApplicationWorkspace>> GetAsync(string fullPath, IAccount executingAccount);
    public Task<Result> OpenAsync(string fullPath, IAccount executingAccount);
    public Task<Result<TApplicationWorkspace>> CreateAsync(string fullPath, IAccount executingAccount);
    public Task<Result<TApplicationWorkspace>> CreateEncryptedAsync(string fullPath, IAccount executingAccount);

    public bool IsOwner(TApplicationWorkspace workspace, IAccount executingAccount);
    public Result ShareAccess(TApplicationWorkspace workspace, params IAccountInfo[] accounts);
    public Result RevokeAccess(TApplicationWorkspace workspace, params IAccountInfo[] accounts);
}
