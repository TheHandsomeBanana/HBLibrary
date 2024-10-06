using HBLibrary.Common.Account;
using HBLibrary.Common.Exceptions;
using HBLibrary.Common.Security.Keys;
using HBLibrary.Common.Security.Rsa;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HBLibrary.Common.Workspace;
public class ApplicationWorkspace : IDisposable {
    public AccountInfo Owner { get; private set; }
    public IReadOnlyList<AccountInfo> SharedAccess { get; private set; } = [];
    public string FullPath { get; set; } = "";
    public bool UsesEncryption { get; set; }

    [JsonIgnore]
    private string? name;
    private bool disposedValue;

    [JsonIgnore]
    public string Name {
        get {
            name ??= Path.GetFileNameWithoutExtension(FullPath);

            return name;
        }
    }

    [JsonIgnore]
    public Account.Account OpenedBy { get; set; }

    [JsonConstructor]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public ApplicationWorkspace() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    internal ApplicationWorkspace(string fullPath, bool usesEncryption, Account.Account openedBy, AccountInfo owner) {
        this.UsesEncryption = usesEncryption;
        this.FullPath = fullPath;
        this.OpenedBy = openedBy;
        this.Owner = owner;
    }

    protected async Task<Result<AesKey>> GetKeyAsync() {
        if(!UsesEncryption) {
            return new ApplicationWorkspaceException("Workspaces does not use encryption");
        }

        string workspaceKeyPath = Path.Combine(
                GlobalEnvironment.ApplicationDataBasePath,
                OpenedBy.Application,
                "workspaces",
                Path.GetFileNameWithoutExtension(FullPath),
                OpenedBy.AccountId
            );

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
            return ApplicationWorkspaceException.CannotOpen("data is corrupted");
        }

        return workspaceAesKey;
    }




    protected virtual void Dispose(bool disposing) {
        if (!disposedValue) {
            if (disposing) {
                // TODO: dispose managed state (managed objects)
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            OpenedBy = null;
            Owner = null;
            SharedAccess = null;
            name = null;
            disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~ApplicationWorkspace()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose() {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
