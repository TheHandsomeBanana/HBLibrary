﻿using HBLibrary.Common.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO;
public interface IFileEntryService {
    Task CopyFileAsync(FileSnapshot source, DirectorySnapshot target, CopyOperationAction action = CopyOperationAction.Skip);
    Task MoveFileAsync(FileSnapshot source, DirectorySnapshot target, MoveOperationAction action = MoveOperationAction.Skip);
    Task ReplaceFileAsync(FileSnapshot source, FileSnapshot target);

    Task CopyDirectoryAsync(DirectorySnapshot source, DirectorySnapshot target, CopyOperationAction action = CopyOperationAction.Skip);
    Task MoveDirectoryAsync(DirectorySnapshot source, DirectorySnapshot target, MoveOperationAction action = MoveOperationAction.Skip);
    Task ReplaceDirectoryAsync(DirectorySnapshot source, DirectorySnapshot target);
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
