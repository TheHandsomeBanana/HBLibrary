using HBLibrary.Core;
using HBLibrary.DataStructures;
using HBLibrary.Interface.Security.Account;
using HBLibrary.Interface.Security.Keys;
using HBLibrary.Interface.Workspace;
using HBLibrary.Security;
using HBLibrary.Security.Account;
using HBLibrary.Security.Rsa;
using HBLibrary.Workspace.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HBLibrary.Workspace;
public class ApplicationWorkspaceManager : IApplicationWorkspaceManager {
    private readonly IAccountStorage accountStorage;
    public string Application { get; }
    public IApplicationWorkspace? CurrentWorkspace { get; private set; }
    public event Action? OnWorkspaceOpened;

    public ApplicationWorkspaceManager(string application, IAccountStorage accountStorage) {
        Application = application;
        this.accountStorage = accountStorage;
    }

    public async Task<bool> WorkspaceExistsAsync(string fullPath) {
        if (!File.Exists(fullPath)) {
            return false;
        }

        Result<ApplicationWorkspace> deserializeResult = await Deserialize<ApplicationWorkspace>(fullPath);

        return deserializeResult.Match(e => true, e => false);
    }



    public async Task<Result<TApplicationWorkspace>> GetAsync<TApplicationWorkspace>(string fullPath, IAccount executingAccount) where TApplicationWorkspace : IApplicationWorkspace {
        if (!File.Exists(fullPath)) {
            return ApplicationWorkspaceException.DoesNotExist();
        }

        try {
            Result<TApplicationWorkspace> deserializeResult = await DeserializeAndCheckAccount<TApplicationWorkspace>(fullPath, executingAccount);
            return deserializeResult;
        }
        catch (Exception ex) {
            return ex;
        }
    }



    public async Task<Result> OpenAsync<TApplicationWorkspace>(string fullPath, IAccount executingAccount) where TApplicationWorkspace : IApplicationWorkspace {
        if (!File.Exists(fullPath)) {
            return ApplicationWorkspaceException.DoesNotExist();
        }

        try {
            Result<TApplicationWorkspace> deserializeResult = await DeserializeAndCheckAccount<TApplicationWorkspace>(fullPath, executingAccount);

            return await deserializeResult.MatchAsync<Result>(async e => {
                CurrentWorkspace = e;
                await CurrentWorkspace.OpenAsync(executingAccount);
                OnWorkspaceOpened?.Invoke();
                return Result.Ok();

            }, async e => await Task.FromResult(e));
        }
        catch (Exception ex) {
            return ex;
        }
    }

    private static async Task<Result<TApplicationWorkspace>> Deserialize<TApplicationWorkspace>(string fullPath) where TApplicationWorkspace : IApplicationWorkspace {
        try {
            byte[] workspace = await UnifiedFile.ReadAllBytesAsync(fullPath);
            TApplicationWorkspace? applicationWorkspace = JsonSerializer.Deserialize<TApplicationWorkspace>(workspace);

            if (applicationWorkspace is null) {
                return ApplicationWorkspaceException.CannotGet("data is corrupted");
            }

            return applicationWorkspace;
        }
        catch (Exception ex) {
            return ex;
        }
    }

    private async Task<Result<TApplicationWorkspace>> DeserializeAndCheckAccount<TApplicationWorkspace>(string fullPath, IAccount executingAccount) where TApplicationWorkspace : IApplicationWorkspace {
        try {
            byte[] workspace = await UnifiedFile.ReadAllBytesAsync(fullPath);
            TApplicationWorkspace? applicationWorkspace = JsonSerializer.Deserialize<TApplicationWorkspace>(workspace);

            if (applicationWorkspace is null) {
                return ApplicationWorkspaceException.CannotGet("data is corrupted");
            }

            IAccountInfo? accountInfo = await accountStorage.GetAccountAsync(executingAccount.AccountId);

            if (accountInfo is null) {
                return ApplicationWorkspaceException.CannotOpen("account information is corrupted");
            }

            if (applicationWorkspace.Owner != accountInfo && applicationWorkspace.SharedAccess.All(e => e != accountInfo)) {
                return ApplicationWorkspaceException.AccessDenied(fullPath);
            }

            return applicationWorkspace;
        }
        catch (Exception ex) {
            return ex;
        }
    }

    public async Task<Result<TApplicationWorkspace>> CreateAsync<TApplicationWorkspace>(string fullPath, IAccount executingAccount) where TApplicationWorkspace : IApplicationWorkspace, new() {
        if (File.Exists(fullPath)) {
            return new InvalidOperationException("Workspace already exists.");
        }

        IAccountInfo? accountInfo = accountStorage.GetAccount(executingAccount.AccountId);

        if (accountInfo is null) {
            return new InvalidOperationException($"Cannot create workspace, account information corrupted");
        }

        TApplicationWorkspace workspace = new TApplicationWorkspace {
            Owner = accountInfo,
            FullPath = fullPath,
            UsesEncryption = false
        };

        workspace.OnCreated();

        string serializedWorkspace = JsonSerializer.Serialize(workspace);

        await UnifiedFile.WriteAllTextAsync(fullPath, serializedWorkspace);
        return workspace;
    }



    public async Task<Result<TApplicationWorkspace>> CreateEncryptedAsync<TApplicationWorkspace>(string fullPath, IAccount executingAccount) where TApplicationWorkspace : IApplicationWorkspace, new() {
        try {
            IAccountInfo? accountInfo = await accountStorage.GetAccountAsync(executingAccount.AccountId);
            if (accountInfo is null) {
                return ApplicationWorkspaceException.CannotOpen("account information is corrupted");
            }

            AesKey aesKey = KeyGenerator.GenerateAesKey();
            string workspaceKeyPath = WorkspaceHelper.GetWorkspaceKeyPath(Application, fullPath, executingAccount.AccountId);


            byte[] workspaceKey = GlobalEnvironment.Encoding.GetBytes(JsonSerializer.Serialize(aesKey));
            byte[] encryptedWorkspaceKey = new RsaCryptographer().Encrypt(workspaceKey, executingAccount.PublicKey);

            await UnifiedFile.WriteAllBytesAsync(workspaceKeyPath, encryptedWorkspaceKey);

            TApplicationWorkspace workspace = new TApplicationWorkspace {
                Owner = accountInfo,
                FullPath = fullPath,
                UsesEncryption = true
            };

            workspace.OnCreated();

            byte[] serializedWorkspace = GlobalEnvironment.Encoding.GetBytes(JsonSerializer.Serialize(workspace));

            await UnifiedFile.WriteAllBytesAsync(fullPath, serializedWorkspace);
            return workspace;
        }
        catch (Exception ex) {
            return ex;
        }
    }

    public Result ShareAccess(IApplicationWorkspace workspace, params IAccountInfo[] accounts) {
        if (!workspace.IsOpen) {
            return ApplicationWorkspaceException.NotOpened(workspace.Name!);
        }

        if (workspace.OpenedBy!.AccountId != workspace.Owner!.AccountId) {
            return new ApplicationWorkspaceException("Not authorized to share access");
        }

        IAccountInfo[] distinctNewAccounts = accounts.Where(e =>
            workspace.Owner.AccountId != e.AccountId &&
            workspace.SharedAccess.All(f => f.AccountId != e.AccountId)
        ).ToArray();

        workspace.SharedAccess = [.. workspace.SharedAccess.Concat(distinctNewAccounts)];

        if (!workspace.UsesEncryption) {
            return Result.Ok();
        }

        Result<byte[]> workspaceKeyResult = WorkspaceHelper.GetWorkspaceKeyBytes(workspace.OpenedBy, workspace.FullPath!);
        if (workspaceKeyResult.IsFaulted) {
            return workspaceKeyResult.Error!;
        }

        AesKey? workspaceAesKey = JsonSerializer.Deserialize<AesKey>(workspaceKeyResult.Value!);
        if (workspaceAesKey is null) {
            return new ApplicationWorkspaceException("Key data is corrupted");
        }

        foreach (IAccountInfo account in distinctNewAccounts) {
            Result<RsaKey> publicKeyGetResult = account.GetPublicKey();
            if (publicKeyGetResult.IsFaulted) {
                return publicKeyGetResult.Error!;
            }

            string newWorkspaceKeyPath = WorkspaceHelper.GetWorkspaceKeyPath(Application, workspace.FullPath!, account.AccountId);
            byte[] newEncryptedWorkspaceKey = new RsaCryptographer().Encrypt(workspaceKeyResult.Value!, publicKeyGetResult.Value!);
            File.WriteAllBytes(newWorkspaceKeyPath, newEncryptedWorkspaceKey);
        }

        return Result.Ok();
    }

    public Result RevokeAccess(IApplicationWorkspace workspace, params IAccountInfo[] accounts) {
        if (!workspace.IsOpen) {
            return ApplicationWorkspaceException.NotOpened(workspace.Name!);
        }

        if (workspace.OpenedBy!.AccountId != workspace.Owner!.AccountId) {
            return new ApplicationWorkspaceException("Not authorized to share access");
        }

        IAccountInfo[] distinctRevokableAccounts = accounts.Where(e =>
            workspace.Owner.AccountId != e.AccountId &&
            workspace.SharedAccess.Any(f => f.AccountId == e.AccountId)
        ).ToArray();

        workspace.SharedAccess = [.. workspace.SharedAccess.Except(distinctRevokableAccounts)];

        if (!workspace.UsesEncryption) {
            return Result.Ok();
        }

        foreach (IAccountInfo accountInfo in distinctRevokableAccounts) {
            string workspaceKeyPath = WorkspaceHelper.GetWorkspaceKeyPath(Application, workspace.FullPath!, accountInfo.AccountId);

            if (File.Exists(workspaceKeyPath)) {
                File.Delete(workspaceKeyPath);
            }
        }

        return Result.Ok();
    }

    public bool IsOwner(IApplicationWorkspace workspace, IAccount executingAccount) {
        IAccountInfo? accountInfo = accountStorage.GetAccount(executingAccount.AccountId);
        return workspace.Owner == accountInfo;
    }
}

public class ApplicationWorkspaceManager<TApplicationWorkspace> : IApplicationWorkspaceManager<TApplicationWorkspace> where TApplicationWorkspace : class, IApplicationWorkspace, new() {
    private readonly ApplicationWorkspaceManager internalWorkspaceManager;
    public string Application => internalWorkspaceManager.Application;
    public TApplicationWorkspace? CurrentWorkspace => internalWorkspaceManager.CurrentWorkspace as TApplicationWorkspace;
    public event Action? OnWorkspaceOpened;

    public ApplicationWorkspaceManager(string application, IAccountStorage accountStorage) {
        internalWorkspaceManager = new ApplicationWorkspaceManager(application, accountStorage);
        internalWorkspaceManager.OnWorkspaceOpened += OnWorkspaceOpened;
    }

    public Task<bool> WorkspaceExistsAsync(string fullPath) {
        return internalWorkspaceManager.WorkspaceExistsAsync(fullPath);
    }

    public Task<Result<TApplicationWorkspace>> GetAsync(string fullPath, IAccount executingAccount) {
        return internalWorkspaceManager.GetAsync<TApplicationWorkspace>(fullPath, executingAccount);
    }

    public Task<Result> OpenAsync(string fullPath, IAccount executingAccount) {
        return internalWorkspaceManager.OpenAsync<TApplicationWorkspace>(fullPath, executingAccount);
    }

    public Task<Result<TApplicationWorkspace>> CreateAsync(string fullPath, IAccount executingAccount) {
        return internalWorkspaceManager.CreateAsync<TApplicationWorkspace>(fullPath, executingAccount);
    }

    public Task<Result<TApplicationWorkspace>> CreateEncryptedAsync(string fullPath, IAccount executingAccount) {
        return internalWorkspaceManager.CreateEncryptedAsync<TApplicationWorkspace>(fullPath, executingAccount);
    }

    public Result ShareAccess(TApplicationWorkspace workspace, params IAccountInfo[] accounts) {
        return internalWorkspaceManager.ShareAccess(workspace, accounts);
    }

    public Result RevokeAccess(TApplicationWorkspace workspace, params IAccountInfo[] accounts) {
        return internalWorkspaceManager.RevokeAccess(workspace, accounts);
    }

    public bool IsOwner(TApplicationWorkspace workspace, IAccount executingAccount) {
        return internalWorkspaceManager.IsOwner(workspace, executingAccount);
    }
}
