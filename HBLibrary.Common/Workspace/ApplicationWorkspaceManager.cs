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
        try {
            byte[] workspace = await UnifiedFile.ReadAllBytesAsync(fullPath);
            ApplicationWorkspace? applicationWorkspace = JsonSerializer.Deserialize<ApplicationWorkspace>(workspace);
            return applicationWorkspace is not null;
        }
        catch {
            return false;
        }
    }

    public async Task<Result<TApplicationWorkspace>> GetAsync<TApplicationWorkspace>(string fullPath, Account.Account executingAccount) where TApplicationWorkspace : ApplicationWorkspace {
        if (!File.Exists(fullPath)) {
            return ApplicationWorkspaceException.DoesNotExist();
        }


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

    public async Task<Result> OpenAsync<TApplicationWorkspace>(string fullPath, Account.Account executingAccount) where TApplicationWorkspace : ApplicationWorkspace {
        if (!File.Exists(fullPath)) {
            return ApplicationWorkspaceException.DoesNotExist();
        }

        try {
            byte[] workspace = await UnifiedFile.ReadAllBytesAsync(fullPath);

            TApplicationWorkspace? applicationWorkspace = JsonSerializer.Deserialize<TApplicationWorkspace>(workspace);
            if (applicationWorkspace is null) {
                return ApplicationWorkspaceException.CannotOpen("data is corrupted");
            }

            AccountInfo? accountInfo = await accountStorage.GetAccountAsync(executingAccount.AccountId);

            if (accountInfo is null) {
                return ApplicationWorkspaceException.CannotOpen("account information is corrupted");
            }

            if (applicationWorkspace.Owner != accountInfo && applicationWorkspace.SharedAccess.All(e => e != accountInfo)) {
                return ApplicationWorkspaceException.AccessDenied(fullPath);
            }

            CurrentWorkspace = applicationWorkspace;
            await CurrentWorkspace.OpenAsync(executingAccount);
            OnWorkspaceOpened?.Invoke();
            return Result.Ok();
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

            Result<RsaKey> accountPrivateKeyResult = await executingAccount.GetPrivateKeyAsync();
            RsaKey accountPrivateKey = accountPrivateKeyResult.GetValueOrThrow();

            AesKey aesKey = KeyGenerator.GenerateAesKey();
            string workspaceKeyPath = Path.Combine(
                GlobalEnvironment.ApplicationDataBasePath,
                Application,
                "workspaces",
                Path.GetFileNameWithoutExtension(fullPath),
                executingAccount.AccountId
            );

            byte[] workspaceKey = GlobalEnvironment.Encoding.GetBytes(JsonSerializer.Serialize(aesKey));
            byte[] encryptedWorkspaceKey = new RsaCryptographer().Encrypt(workspaceKey, accountPrivateKey);

            await UnifiedFile.WriteAllBytesAsync(workspaceKeyPath, encryptedWorkspaceKey);

            TApplicationWorkspace workspace = new TApplicationWorkspace {
                Owner = accountInfo,
                FullPath = fullPath,
                UsesEncryption = true
            };

            workspace.OnCreated();

            await workspace.OpenAsync(executingAccount);
            byte[] serializedWorkspace = GlobalEnvironment.Encoding.GetBytes(JsonSerializer.Serialize(workspace));

            await UnifiedFile.WriteAllBytesAsync(fullPath, serializedWorkspace);
            return workspace;
        }
        catch (Exception ex) {
            return ex;
        }
    }

    public async Task<Result> ShareAccess(ApplicationWorkspace workspace, params AccountInfo[] accounts) {
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

        foreach (AccountInfo account in distinctNewAccounts) {
            // TODO: Share new access request with user
            // -> Create notification logic
            // --> Notify other when logged in about 
            // --> On accept generate new workspace key
        }

        return Result.Ok();
    }

    public async Task<Result> RevokeAccess(ApplicationWorkspace workspace, params AccountInfo[] accounts) {
        if (!workspace.IsOpen) {
            return ApplicationWorkspaceException.NotOpened(workspace.Name!);
        }

        if (workspace.OpenedBy!.AccountId != workspace.Owner!.AccountId) {
            return new ApplicationWorkspaceException("Not authorized to share access");
        }

        AccountInfo[] distinctRevokableAccounts = accounts.Where(e =>
            workspace.Owner.AccountId != e.AccountId &&
            workspace.SharedAccess.All(f => f.AccountId != e.AccountId)
        ).ToArray();

        workspace.SharedAccess = [.. workspace.SharedAccess.Except(distinctRevokableAccounts)];

        if (!workspace.UsesEncryption) {
            return Result.Ok();
        }

        foreach (AccountInfo accountInfo in distinctRevokableAccounts) {
            string workspaceKeyPath = Path.Combine(
                GlobalEnvironment.ApplicationDataBasePath,
                Application,
                "workspaces",
                workspace.Name!,
                accountInfo.AccountId
            );

            if (File.Exists(workspaceKeyPath)) {
                File.Delete(workspaceKeyPath);
            }
        }

        return Result.Ok();
    }
}

public class ApplicationWorkspaceManager<TApplicationWorkspace> : IApplicationWorkspaceManager<TApplicationWorkspace> where TApplicationWorkspace : ApplicationWorkspace, new() {
    private readonly IAccountStorage accountStorage;
    public string Application { get; }
    public TApplicationWorkspace? CurrentWorkspace { get; private set; }
    public event Action? OnWorkspaceOpened;
    public ApplicationWorkspaceManager(string application, IAccountStorage accountStorage) {
        this.Application = application;
        this.accountStorage = accountStorage;
    }

    public async Task<bool> WorkspaceExistsAsync(string fullPath) {
        if (!File.Exists(fullPath)) {
            return false;
        }
        try {
            byte[] workspace = await UnifiedFile.ReadAllBytesAsync(fullPath);
            ApplicationWorkspace? applicationWorkspace = JsonSerializer.Deserialize<ApplicationWorkspace>(workspace);
            return applicationWorkspace is not null;
        }
        catch {
            return false;
        }
    }

    public async Task<Result<TApplicationWorkspace>> GetAsync(string fullPath, Account.Account executingAccount) {
        if (!File.Exists(fullPath)) {
            return ApplicationWorkspaceException.DoesNotExist();
        }


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

    public async Task<Result> OpenAsync(string fullPath, Account.Account executingAccount) {
        if (!File.Exists(fullPath)) {
            return ApplicationWorkspaceException.DoesNotExist();
        }

        try {
            byte[] workspace = await UnifiedFile.ReadAllBytesAsync(fullPath);

            TApplicationWorkspace? applicationWorkspace = JsonSerializer.Deserialize<TApplicationWorkspace>(workspace);
            if (applicationWorkspace is null) {
                return ApplicationWorkspaceException.CannotOpen("data is corrupted");
            }

            AccountInfo? accountInfo = await accountStorage.GetAccountAsync(executingAccount.AccountId);

            if (accountInfo is null) {
                return ApplicationWorkspaceException.CannotOpen("account information is corrupted");
            }

            if (applicationWorkspace.Owner != accountInfo && applicationWorkspace.SharedAccess.All(e => e != accountInfo)) {
                return ApplicationWorkspaceException.AccessDenied(fullPath);
            }

            CurrentWorkspace = applicationWorkspace;
            await CurrentWorkspace.OpenAsync(executingAccount);
            OnWorkspaceOpened?.Invoke();
            return Result.Ok();
        }
        catch (Exception ex) {
            return ex;
        }
    }

    public async Task<Result<TApplicationWorkspace>> CreateAsync(string fullPath, Account.Account executingAccount) {
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

    public async Task<Result<TApplicationWorkspace>> CreateEncryptedAsync(string fullPath, Account.Account executingAccount) {
        try {
            AccountInfo? accountInfo = await accountStorage.GetAccountAsync(executingAccount.AccountId);
            if (accountInfo is null) {
                return ApplicationWorkspaceException.CannotOpen("account information is corrupted");
            }

            Result<RsaKey> accountPrivateKeyResult = await executingAccount.GetPrivateKeyAsync();
            RsaKey accountPrivateKey = accountPrivateKeyResult.GetValueOrThrow();

            AesKey aesKey = KeyGenerator.GenerateAesKey();
            string workspaceKeyPath = Path.Combine(
                GlobalEnvironment.ApplicationDataBasePath,
                Application,
                "workspaces",
                Path.GetFileNameWithoutExtension(fullPath),
                executingAccount.AccountId
            );

            byte[] workspaceKey = GlobalEnvironment.Encoding.GetBytes(JsonSerializer.Serialize(aesKey));
            byte[] encryptedWorkspaceKey = new RsaCryptographer().Encrypt(workspaceKey, accountPrivateKey);

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

    public async Task<Result> ShareAccess(TApplicationWorkspace workspace, params AccountInfo[] accounts) {
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

        foreach (AccountInfo account in distinctNewAccounts) {
            // TODO: Share new access request with user
            // -> Create notification logic
            // --> Notify other when logged in about 
            // --> On accept generate new workspace key
        }

        return Result.Ok();
    }

    public async Task<Result> RevokeAccess(TApplicationWorkspace workspace, params AccountInfo[] accounts) {
        if (!workspace.IsOpen) {
            return ApplicationWorkspaceException.NotOpened(workspace.Name!);
        }

        if (workspace.OpenedBy!.AccountId != workspace.Owner!.AccountId) {
            return new ApplicationWorkspaceException("Not authorized to share access");
        }

        AccountInfo[] distinctRevokableAccounts = accounts.Where(e =>
            workspace.Owner.AccountId != e.AccountId &&
            workspace.SharedAccess.All(f => f.AccountId != e.AccountId)
        ).ToArray();

        workspace.SharedAccess = [.. workspace.SharedAccess.Except(distinctRevokableAccounts)];

        if (!workspace.UsesEncryption) {
            return Result.Ok();
        }

        foreach (AccountInfo accountInfo in distinctRevokableAccounts) {
            string workspaceKeyPath = Path.Combine(
                GlobalEnvironment.ApplicationDataBasePath,
                Application,
                "workspaces",
                workspace.Name!,
                accountInfo.AccountId
            );

            if (File.Exists(workspaceKeyPath)) {
                File.Delete(workspaceKeyPath);
            }
        }

        return Result.Ok();
    }
}
