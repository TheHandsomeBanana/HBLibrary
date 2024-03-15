using HBLibrary.Common.Limiter;
using HBLibrary.Common.RegularExpressions;
using HBLibrary.Services.IO.Exceptions;
using System.Collections.Immutable;
using System.Text;
using System.Text.RegularExpressions;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands;
public abstract class WinRARCommand : IWinRARCommand {
    public virtual WinRARCommandName Command { get; }
    public required string TargetArchive { get; init; }
    public WinRARProcessPriority? Priority { get; init; } = null;
    public WinRARFileNameFormat? FileNameFormat { get; init; } = null;
    public bool IgnoreFileAttributes { get; set; } = false;

    public virtual string ToCommandString() {
        StringBuilder sb = new StringBuilder();
        sb.Append(Get(Command))
            .Append(' ')
            .Append(BuildSwitches())
            .Append(' ')
            .Append(TargetArchive);

        return sb.ToString();
    }

    public virtual string BuildSwitches() {
        StringBuilder sb = new StringBuilder();
        if (Priority != null)
            sb.Append(Priority.ToString());

        if (FileNameFormat != null) {
            switch (FileNameFormat) {
                case WinRARFileNameFormat.Uppercase:
                    sb.Append("-cu ");
                    break;
                case WinRARFileNameFormat.Lowercase:
                    sb.Append("-cl ");
                    break;
            }
        }

        if (IgnoreFileAttributes)
            sb.Append("-ai ");

        return sb.ToString();
    }

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

public readonly partial struct WinRARPassword {
    public string Password { get; }
    public WinRARPasswordMode Mode { get; }

    public WinRARPassword(string password, WinRARPasswordMode mode) {
        if (!RegexCollection.CommonCLIPasswordRegex.Match(password).Success)
            throw new ArgumentException($"The provided password does not match the requirements.");

        Password = password;
        Mode = mode;
    }

    public override string ToString() {
        switch (Mode) {
            case WinRARPasswordMode.Basic:
                return "-p";
            case WinRARPasswordMode.EncryptAll:
                return "-hp";
            default:
                throw new NotSupportedException(Mode.ToString());
        }
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
        if (sizeType == WinRARSizeType.Percentage)
            size.LimitToRangeRef(0, 100);

        SizeType = sizeType;
        Size = size;
    }

    public override string ToString() {
        const string rv = "-rv";
        switch (SizeType) {
            case WinRARSizeType.kB:
                return rv + Size + "k";
            case WinRARSizeType.MB:
                return rv + Size + "m";
            case WinRARSizeType.GB:
                return rv + Size + "g";
            case WinRARSizeType.Percentage:
                return rv + Size + "p";
        }

        throw new NotSupportedException(SizeType.ToString());
    }
}

public readonly struct WinRARDataRecoveryRecord {
    public WinRARSizeType SizeType { get; }
    public int Size { get; }

    public WinRARDataRecoveryRecord(WinRARSizeType sizeType, int size) {
        if (sizeType == WinRARSizeType.Percentage)
            size.LimitToRangeRef(0, 100);

        SizeType = sizeType;
        Size = size;
    }

    public override string ToString() {
        const string rv = "-rr";
        switch (SizeType) {
            case WinRARSizeType.kB:
                return rv + Size + "k";
            case WinRARSizeType.MB:
                return rv + Size + "m";
            case WinRARSizeType.Percentage:
                return rv + Size + "%";
        }

        throw new NotSupportedException(SizeType.ToString());
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

public readonly struct WinRARProcessPriority {
    /// <summary>
    /// Valid values are usually [1-15]
    /// </summary>
    public int Priority { get; }
    public TimeSpan? OptionalWait { get; }

    public WinRARProcessPriority(int priority, TimeSpan? optionalWait = null) {
        this.Priority = priority.LimitToRange(0, 15);
        OptionalWait = optionalWait;
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

public enum WinRARFileNameFormat {
    Uppercase,
    Lowercase
}
