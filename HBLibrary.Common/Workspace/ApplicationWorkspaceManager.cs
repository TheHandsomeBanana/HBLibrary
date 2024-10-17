using HBLibrary.Common.Account;
using HBLibrary.Common.Exceptions;
using HBLibrary.Common.Extensions;
using HBLibrary.Common.Security;
using HBLibrary.Common.Security.Aes;
using HBLibrary.Common.Security.Keys;
using HBLibrary.Common.Security.Rsa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HBLibrary.Common.Workspace;
public class ApplicationWorkspaceManager : IApplicationWorkspaceManager {
    private readonly IAccountStorage accountStorage;
    public string Application { get; }
    public ApplicationWorkspace? CurrentWorkspace { get; private set; }
    public event Action? OnWorkspaceOpened;

    public ApplicationWorkspaceManager(string application, IAccountStorage accountStorage) {
        this.Application = application;
        this.accountStorage = accountStorage;
    }

    public async Task<bool> WorkspaceExistsAsync(string fullPath) {
        if (!File.Exists(fullPath)) {
            return false;
        }

        Result<ApplicationWorkspace> deserializeResult = await Deserialize<ApplicationWorkspace>(fullPath);

        return deserializeResult.Match(e => true, e => false);
    }

    

    public async Task<Result<TApplicationWorkspace>> GetAsync<TApplicationWorkspace>(string fullPath, Account.Account executingAccount) where TApplicationWorkspace : ApplicationWorkspace {
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

    

    public async Task<Result> OpenAsync<TApplicationWorkspace>(string fullPath, Account.Account executingAccount) where TApplicationWorkspace : ApplicationWorkspace {
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

    private static async Task<Result<TApplicationWorkspace>> Deserialize<TApplicationWorkspace>(string fullPath) where TApplicationWorkspace : ApplicationWorkspace {
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
    
    private async Task<Result<TApplicationWorkspace>> DeserializeAndCheckAccount<TApplicationWorkspace>(string fullPath, Account.Account executingAccount) where TApplicationWorkspace : ApplicationWorkspace {
        try {
            byte[] workspace = await UnifiedFile.ReadAllBytesAsync(fullPath);
            TApplicationWorkspace? applicationWorkspace = JsonSerializer.Deserialize<TApplicationWorkspace>(workspace);

            if (applicationWorkspace is null) {
                return ApplicationWorkspaceException.CannotGet("data is corrupted");
            }

            AccountInfo? accountInfo = await accountStorage.GetAccountAsync(executingAccount.AccountId);

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

    public async Task<Result<TApplicationWorkspace>> CreateAsync<TApplicationWorkspace>(string fullPath, Account.Account executingAccount) where TApplicationWorkspace : ApplicationWorkspace, new() {
        if (File.Exists(fullPath)) {
            return new InvalidOperationException("Workspace already exists.");
        }

        AccountInfo? accountInfo = accountStorage.GetAccount(executingAccount.AccountId);

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

    

    public async Task<Result<TApplicationWorkspace>> CreateEncryptedAsync<TApplicationWorkspace>(string fullPath, Account.Account executingAccount) where TApplicationWorkspace : ApplicationWorkspace, new() {
        try {
            AccountInfo? accountInfo = await accountStorage.GetAccountAsync(executingAccount.AccountId);
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

    public Result ShareAccess(ApplicationWorkspace workspace, params AccountInfo[] accounts) {
        if (!workspace.IsOpen) {
            return ApplicationWorkspaceException.NotOpened(workspace.Name!);
        }

        if (workspace.OpenedBy!.AccountId != workspace.Owner!.AccountId) {
            return new ApplicationWorkspaceException("Not authorized to share access");
        }

        AccountInfo[] distinctNewAccounts = accounts.Where(e =>
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

        foreach (AccountInfo account in distinctNewAccounts) {
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

    public Result RevokeAccess(ApplicationWorkspace workspace, params AccountInfo[] accounts) {
        if (!workspace.IsOpen) {
            return ApplicationWorkspaceException.NotOpened(workspace.Name!);
        }

        if (workspace.OpenedBy!.AccountId != workspace.Owner!.AccountId) {
            return new ApplicationWorkspaceException("Not authorized to share access");
        }

        AccountInfo[] distinctRevokableAccounts = accounts.Where(e =>
            workspace.Owner.AccountId != e.AccountId &&
            workspace.SharedAccess.Any(f => f.AccountId == e.AccountId)
        ).ToArray();

        workspace.SharedAccess = [.. workspace.SharedAccess.Except(distinctRevokableAccounts)];

        if (!workspace.UsesEncryption) {
            return Result.Ok();
        }

        foreach (AccountInfo accountInfo in distinctRevokableAccounts) {
            string workspaceKeyPath = WorkspaceHelper.GetWorkspaceKeyPath(Application, workspace.FullPath!, accountInfo.AccountId);

            if (File.Exists(workspaceKeyPath)) {
                File.Delete(workspaceKeyPath);
            }
        }

        return Result.Ok();
    }
}

public class ApplicationWorkspaceManager<TApplicationWorkspace> : IApplicationWorkspaceManager<TApplicationWorkspace> where TApplicationWorkspace : ApplicationWorkspace, new() {
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

    public Task<Result<TApplicationWorkspace>> GetAsync(string fullPath, Account.Account executingAccount) {
        return internalWorkspaceManager.GetAsync<TApplicationWorkspace>(fullPath, executingAccount);
    }

    public Task<Result> OpenAsync(string fullPath, Account.Account executingAccount) {
        return internalWorkspaceManager.OpenAsync<TApplicationWorkspace>(fullPath, executingAccount);
    }

    public Task<Result<TApplicationWorkspace>> CreateAsync(string fullPath, Account.Account executingAccount) {
        return internalWorkspaceManager.CreateAsync<TApplicationWorkspace>(fullPath, executingAccount);
    }

    public Task<Result<TApplicationWorkspace>> CreateEncryptedAsync(string fullPath, Account.Account executingAccount) {
        return internalWorkspaceManager.CreateEncryptedAsync<TApplicationWorkspace>(fullPath, executingAccount); 
    }

    public Result ShareAccess(TApplicationWorkspace workspace, params AccountInfo[] accounts) {
        return internalWorkspaceManager.ShareAccess(workspace, accounts);
    }

    public Result RevokeAccess(TApplicationWorkspace workspace, params AccountInfo[] accounts) {
        return internalWorkspaceManager.RevokeAccess(workspace, accounts);
    }
}
