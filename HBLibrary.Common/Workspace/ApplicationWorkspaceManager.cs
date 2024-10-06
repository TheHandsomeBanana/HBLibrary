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

    public async Task<Result<ApplicationWorkspace>> CreateAsync(string fullPath, Account.Account executingAccount) {
        if (File.Exists(fullPath)) {
            return new InvalidOperationException("Workspace already exists.");
        }

        AccountInfo? accountInfo = accountStorage.GetAccount(executingAccount.AccountId);

        if (accountInfo is null) {
            return new InvalidOperationException($"Cannot create workspace, account information corrupted");
        }

        ApplicationWorkspace workspace = new ApplicationWorkspace(fullPath, false, executingAccount, accountInfo);
        string serializedWorkspace = JsonSerializer.Serialize(workspace);

        await UnifiedFile.WriteAllTextAsync(fullPath, serializedWorkspace);

        return workspace;
    }

    public async Task<Result<ApplicationWorkspace>> OpenAsync(string fullPath, Account.Account executingAccount) {
        if (!File.Exists(fullPath)) {
            return ApplicationWorkspaceException.DoesNotExist();
        }

        try {
            byte[] workspace = await UnifiedFile.ReadAllBytesAsync(fullPath);

            ApplicationWorkspace? applicationWorkspace = JsonSerializer.Deserialize<ApplicationWorkspace>(workspace);
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

            return applicationWorkspace;
        }
        catch (Exception ex) {
            return ex;
        }
    }

    public async Task<Result<ApplicationWorkspace>> CreateEncryptedAsync(string fullPath, Account.Account executingAccount) {
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

            ApplicationWorkspace workspace = new ApplicationWorkspace(fullPath, true, executingAccount, accountInfo);
            byte[] serializedWorkspace = GlobalEnvironment.Encoding.GetBytes(JsonSerializer.Serialize(workspace));

            await UnifiedFile.WriteAllBytesAsync(fullPath, serializedWorkspace);
            return workspace;
        }
        catch (Exception ex) {
            return ex;
        }
    }


    public async Task<Result<ApplicationWorkspace>> OpenEncryptedAsync(string fullPath, Account.Account executingAccount) {
        if (!File.Exists(fullPath)) {
            return ApplicationWorkspaceException.DoesNotExist();
        }

        try {
            string workspaceKeyPath = Path.Combine(
                GlobalEnvironment.ApplicationDataBasePath,
                Application,
                "workspaces",
                Path.GetFileNameWithoutExtension(fullPath),
                executingAccount.AccountId
            );

            if (!File.Exists(workspaceKeyPath)) {
                return ApplicationWorkspaceException.CannotOpen("data is corrupted");
            }

            byte[] encryptedWorkspaceKey = await UnifiedFile.ReadAllBytesAsync(workspaceKeyPath);

            Result<RsaKey> privateKeyResult = await executingAccount.GetPrivateKeyAsync();
            RsaKey privateKey = privateKeyResult.GetValueOrThrow();

            byte[] workspaceKey = new RsaCryptographer().Decrypt(encryptedWorkspaceKey, privateKey);

            AesKey? workspaceAesKey = JsonSerializer.Deserialize<AesKey>(workspaceKey);
            if (workspaceAesKey is null) {
                return ApplicationWorkspaceException.CannotOpen("data is corrupted");
            }

            byte[] workspace = await UnifiedFile.ReadAllBytesAsync(fullPath);

            ApplicationWorkspace? applicationWorkspace = JsonSerializer.Deserialize<ApplicationWorkspace>(workspace);
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

            return applicationWorkspace;
        }
        catch (Exception ex) {
            return ex;
        }
    }

    public Task<Result> ShareAccess(ApplicationWorkspace workspace, params AccountInfo[] accounts) {
        throw new NotImplementedException();
    }

    public Task<Result> RevokeAccess(ApplicationWorkspace workspace, params AccountInfo[] accounts) {
        throw new NotImplementedException();
    }


}
