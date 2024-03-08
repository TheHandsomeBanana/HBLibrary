namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands;
public class WinRARCommand {
    public WinRARCommandName CommandName { get; set; }
    internal WinRARCommand(string command) {
    }
}

public enum WinRARCommandName {
    AddFilesToArchive,
    AddCommentToArchive,
    ChangeArchiveParameters,
    ConvertArchives,
    WriteArchiveCommentToFile,
    DeleteFilesFromArchive,
    ExtractFilesFromArchiveIgnoringPaths,
    FreshenFilesWithinArchive,
    FindStringInArchives,
    LockArchive,
    MoveFilesAndFoldersToArchive,
    RepairDamagedArchive,
    ReconstructMissingVolumes,
    RenameArchivedFiles,
    AddDataRecoveryRecord,
    CreateRecoveryVolumes,
    ConvertArchiveToSelfExtractingType,
    RemoveSFXModule,
    TestArchiveFiles,
    UpdateFilesWithinArchive,
    ExtractFilesFromArchiveWithFullPaths
}

public readonly struct WinRARVolumeSize {
    public WinRARSizeType SizeType { get; }
    public int Size { get; }

    public WinRARVolumeSize(WinRARSizeType sizeType, int size) {
        SizeType = sizeType;
        Size = size;
    }


    public static WinRARVolumeSize Email25MB() => new WinRARVolumeSize(WinRARSizeType.MB, 25);
    public static WinRARVolumeSize WebUpload100MB() => new WinRARVolumeSize(WinRARSizeType.MB, 100);
    public static WinRARVolumeSize WebUpload200MB() => new WinRARVolumeSize(WinRARSizeType.MB, 200);

    public override string ToString() {
        switch (SizeType) {
            case WinRARSizeType.kB:
                return "-v" + Size + "k";
            case WinRARSizeType.MB:
                return "-v" + Size + "m";
            case WinRARSizeType.GB:
                return "-v" + Size + "g";
        }

        throw new NotSupportedException(SizeType.ToString());
    }
}

public enum WinRARSizeType {
    kB,
    MB,
    GB,
    Percentage
}

public enum WinRARCompressionLevel {
    Save,
    Fastest,
    Fast,
    Normal,
    Good,
    Best
}

public enum WinRARDictionarySize {
    Md64k,
    Md128k,
    Md256k,
    Md512k,
    Md1024k,
    Md2048k,
    Md4096k,
    Md1m,
    Md2m,
    Md4m,
    Md8m,
    Md16m,
    Md32m,
    Md64m,
    Md128m,
    Md256m,
    Md512m,
    Md1024m
}

public enum WinRAROverwriteMode {
    Overwrite,
    Skip
}

public enum WinRARExtractionMode {
    FullPaths, 
    IgnoreFolderStructure,
}
