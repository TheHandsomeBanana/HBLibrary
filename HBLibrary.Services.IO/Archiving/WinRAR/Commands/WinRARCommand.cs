using System.Collections.Immutable;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands;
public abstract class WinRARCommand {
    protected string? CommandString { get; set; }
    public virtual WinRARCommandName Command { get; }
    public ImmutableArray<string> Arguments { get; }
    public WinRARCommand() {
    }

    public WinRARCommand(string commandString) {
        // Todo: validate commandString
        this.CommandString = commandString;
    }

    public string ToCommandString() {
        return CommandString != null ? CommandString : WinRARCommandMap.Get(Command) + " " + string.Join(" ", Arguments);
    }
}

public static class WinRARCommandMap {
    public const string AddCommand = "a";
    public const string UpdateCommand = "u";
    public const string ExtractFullCommand = "x";
    public const string ExtractCommand = "e";
    public const string CommentCommand = "c";
    public const string RepairCommand = "r";
    public const string DeleteCommand = "d";

    public static string Get(WinRARCommandName commandName) {
        return commandName switch {
            WinRARCommandName.Add => AddCommand,
            WinRARCommandName.Update => UpdateCommand,
            WinRARCommandName.Extract => ExtractCommand,
            WinRARCommandName.ExtractFull => ExtractFullCommand,
            WinRARCommandName.Comment => CommentCommand,
            WinRARCommandName.Repair => RepairCommand,
            WinRARCommandName.Delete => DeleteCommand,
            _ => throw new NotSupportedException(commandName.ToString())
        };
    }
}

public enum WinRARCommandName {
    Add,
    Update,
    Extract,
    ExtractFull,
    Comment,
    Repair,
    Delete
}

public readonly struct WinRARPassword {
    public string Password { get; }
    public WinRARPasswordMode Mode { get; }

    public WinRARPassword(string password, WinRARPasswordMode mode) {
        Password = password;
        Mode = mode;
    }
}

public enum WinRARPasswordMode {
    Basic,
    EncryptAll
}

public readonly struct WinRARRecoveryVolume {
    public WinRARSizeType SizeType { get; }
    public int Size { get; }

    public WinRARRecoveryVolume(WinRARSizeType sizeType, int size) {
        SizeType = sizeType;
        Size = size;
    }

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
    Silent,
    Skip,
}

public enum WinRARExtractionMode {
    FullPaths,
    IgnoreFolderStructure,
}
