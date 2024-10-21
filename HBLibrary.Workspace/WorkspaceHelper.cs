using HBLibrary.Core;
using HBLibrary.Core.Extensions;
using HBLibrary.DataStructures;
using HBLibrary.Interface.Security.Account;
using HBLibrary.Interface.Security.Keys;
using HBLibrary.Security.Rsa;
using HBLibrary.Workspace.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HBLibrary.Workspace;
internal static class WorkspaceHelper {
    internal static string GetWorkspaceKeyPath(string application, string fullPath, string id) {
        string workspaceKeyPath = Path.Combine(
                GlobalEnvironment.ApplicationDataBasePath,
                application,
                "workspaces",
                fullPath.ToGuidString(),
                id + ".id");

        Directory.CreateDirectory(Path.GetDirectoryName(workspaceKeyPath)!);

        return workspaceKeyPath;
    }

    internal static Result<byte[]> GetWorkspaceKeyBytes(IAccount account, string fullPath) {
        string workspaceKeyPath = GetWorkspaceKeyPath(account!.Application, fullPath!, account.AccountId);

        if (!File.Exists(workspaceKeyPath)) {
            return new ApplicationWorkspaceException("Key does not exist");
        }

        byte[] encryptedWorkspaceKey = File.ReadAllBytes(workspaceKeyPath);

        Result<RsaKey> privateKeyResult = account.GetPrivateKey();
        if (privateKeyResult.IsFaulted) {
            return privateKeyResult.Error!;
        }

        byte[] workspaceKey = new RsaCryptographer().Decrypt(encryptedWorkspaceKey, privateKeyResult.Value!);
        return workspaceKey;
    }

    internal static Result<AesKey> GetWorkspaceKey(IAccount account, string fullPath) {
        Result<byte[]> workspaceKeyResult = GetWorkspaceKeyBytes(account, fullPath);

        return workspaceKeyResult.Match<Result<AesKey>>(e => {
            AesKey? workspaceAesKey = JsonSerializer.Deserialize<AesKey>(e);
            if (workspaceAesKey is null) {
                return new ApplicationWorkspaceException("Key data is corrupted");
            }

            return workspaceAesKey;
        }, e => e);
    }
}
