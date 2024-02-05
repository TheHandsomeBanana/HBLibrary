using HBLibrary.Common.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO;
public interface IFileEntryService {
    void CopyFile(string source, string target, CopyOperationAction action = CopyOperationAction.Skip);
    void MoveFile(string source, string target, MoveOperationAction action = MoveOperationAction.Skip);
    Task CopyFileAsync(string source, string target, CopyOperationAction action = CopyOperationAction.Skip);
    Task MoveFileAsync(string source, string target, MoveOperationAction action = MoveOperationAction.Skip);

    void CopyDirectory(string source, string target, CopyOperationAction action = CopyOperationAction.Skip);
    void MoveDirectory(string source, string target, MoveOperationAction action = MoveOperationAction.Skip);
    void ReplaceDirectory(string source, string target);
    Task CopyDirectoryAsync(string source, string target, CopyOperationAction action = CopyOperationAction.Skip);
    Task MoveDirectoryAsync(string source, string target, MoveOperationAction action = MoveOperationAction.Skip);
    Task ReplaceDirectoryAsync(string source, string target);
}

public enum CopyOperationAction {
    Skip,
    OverwriteAll,
    OverwriteModifiedOnly
}

public enum MoveOperationAction {
    Skip,
    Overwrite
}
