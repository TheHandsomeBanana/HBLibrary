using HBLibrary.Common.Account;
using HBLibrary.Common.Exceptions;
using HBLibrary.Common.Extensions;
using HBLibrary.Common.Security.Keys;
using HBLibrary.Common.Security.Rsa;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HBLibrary.Common.Workspace;
public class ApplicationWorkspace {
    public AccountInfo? Owner { get; set; }
    public AccountInfo[] SharedAccess { get; set; } = [];
    public string? FullPath { get; set; }
    public bool UsesEncryption { get; set; }

   
    private string? name;

    [JsonIgnore]
    public string? Name {
        get {
            name ??= Path.GetFileName(FullPath);

            return name;
        }
    }

    [JsonIgnore]
    public bool IsOpen { get; private set; }
    [JsonIgnore]
    public Account.Account? OpenedBy { get; set; }

    public event Action? OnOpened;

    [JsonConstructor]
    public ApplicationWorkspace() { }

    public virtual void OnCreated() { }
    public virtual void Save() { }
    public virtual Task SaveAsync() {
        return Task.CompletedTask;
    }

    protected void NotifyOpened() {
        OnOpened?.Invoke();
    }
   
    public virtual Task OpenAsync(Account.Account openedBy) {
        if (IsOpen) {
            throw ApplicationWorkspaceException.CannotOpen("already opened");
        }

        IsOpen = true;
        OpenedBy = openedBy;
        return Task.CompletedTask;
    }

    public virtual void Close() {
        IsOpen = false;
        OpenedBy = null;
    }

    public virtual Task CloseAsync() {
        IsOpen = false;
        OpenedBy = null;
        return Task.CompletedTask;
    }

    protected Result<AesKey> GetKey() {
        if (!IsOpen) {
            return ApplicationWorkspaceException.NotOpened(Name!);
        }

        if (!UsesEncryption) {
            return new ApplicationWorkspaceException("Workspaces does not use encryption");
        }

        return WorkspaceHelper.GetWorkspaceKey(OpenedBy!, FullPath!);
    }

    protected async Task<Result<AesKey>> GetKeyAsync() {
        if (!IsOpen) {
            return ApplicationWorkspaceException.NotOpened(Name!);
        }

        if(!UsesEncryption) {
            return new ApplicationWorkspaceException("Workspaces does not use encryption");
        }

        string workspaceKeyPath = WorkspaceHelper.GetWorkspaceKeyPath(OpenedBy!.Application, FullPath!, OpenedBy!.AccountId);

        if (!File.Exists(workspaceKeyPath)) {
            return new ApplicationWorkspaceException("Key does not exist");
        }

        byte[] encryptedWorkspaceKey = await UnifiedFile.ReadAllBytesAsync(workspaceKeyPath);

        Result<RsaKey> privateKeyResult = await OpenedBy.GetPrivateKeyAsync();
        if (privateKeyResult.IsFaulted) {
            return privateKeyResult.Error!;
        }

        byte[] workspaceKey = new RsaCryptographer().Decrypt(encryptedWorkspaceKey, privateKeyResult.Value!);

        AesKey? workspaceAesKey = JsonSerializer.Deserialize<AesKey>(workspaceKey);
        if (workspaceAesKey is null) {
            return new ApplicationWorkspaceException("Key data is corrupted");
        }

        return workspaceAesKey;
    }
}
