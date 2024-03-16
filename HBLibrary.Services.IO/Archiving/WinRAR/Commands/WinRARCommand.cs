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
    public WinRARProcessPriority? Priority { get; init; } 
    public WinRARPassword? Password { get; init; }


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
        if (Priority.HasValue)
            sb.Append(Priority.Value.ToString())
                .Append(' ');

        if (Password.HasValue)
            sb.Append(Password.Value.ToString())
                .Append(' ');

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
    /// Valid range is [0-15], with 0 beeing the default, 1 the lowest and 15 the highest priority.
    /// </summary>
    public int Priority { get; }
    /// <summary>
    /// Valid range is [0-1000] in milliseconds.
    /// </summary>
    public TimeSpan? OptionalWait { get; }

    public WinRARProcessPriority(int priority, TimeSpan? optionalWait = null) {
        priority.LimitToRangeRef(0, 15);
        this.Priority = priority;

        if (optionalWait.HasValue)
            optionalWait = optionalWait.Value.LimitToRange(TimeSpan.FromMilliseconds(0), TimeSpan.FromMilliseconds(1000));

        OptionalWait = optionalWait;
    }

    public override string ToString() {
        StringBuilder sb = new StringBuilder()
            .Append("-ri")
            .Append(Priority);

        if (OptionalWait != null) {
            sb.Append(':')
                .Append(OptionalWait.Value.TotalMilliseconds);
        }

        return sb.ToString();
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
    D64Kb,
    D128Kb,
    D256Kb,
    D512Kb,
    D1024Kb,
    D2048Kb,
    D4096Kb,
    D1MB,
    D2MB,
    D4MB,
    D8MB,
    D16MB,
    D32MB,
    D64MB,
    D128MB,
    D256MB,
    D512MB,
    D1024MB
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

public enum AuthenticityVerification {
    Enabled,
    Disabled,
}


public static class WinRARNameMapping {
    public static string Get(WinRARCommandName value)
        => value switch {
            WinRARCommandName.Extract => "e",
            WinRARCommandName.ExtractFull => "x",
            WinRARCommandName.Add => "a",
            WinRARCommandName.Comment => "c",
            WinRARCommandName.Repair => "r",
            WinRARCommandName.Delete => "d",
            WinRARCommandName.Update => "u",
            _ => ""
        };

    public static string Get(WinRARFileNameFormat value)
        => value switch {
            WinRARFileNameFormat.Uppercase => "-cu",
            WinRARFileNameFormat.Lowercase => "-cl",
            _ => ""
        };

    public static string Get(WinRARExtractionMode value)
        => value switch {
            WinRARExtractionMode.IgnoreFolderStructure => "e",
            WinRARExtractionMode.FullPaths => "x",
            _ => ""
        };

    public static string Get(WinRARCompressionLevel value)
        => value switch {
            WinRARCompressionLevel.Save => "-m0",
            WinRARCompressionLevel.Fastest => "-m1",
            WinRARCompressionLevel.Fast => "-m2",
            WinRARCompressionLevel.Normal => "-m3",
            WinRARCompressionLevel.Good => "-m4",
            WinRARCompressionLevel.Best => "-m5",
            _ => ""
        };

    public static string Get(WinRARSizeType value)
        => value switch {
            WinRARSizeType.kB => "k",
            WinRARSizeType.MB => "m",
            WinRARSizeType.GB => "g",
            WinRARSizeType.Percentage => "p",
            _ => ""
        };

    public static string Get(WinRARPasswordMode value)
        => value switch {
            WinRARPasswordMode.Basic => "-p",
            WinRARPasswordMode.EncryptAll => "-hp",
            _ => ""
        };


    public static string Get(AuthenticityVerification value)
        => value switch {
            AuthenticityVerification.Enabled => "-av",
            AuthenticityVerification.Disabled => "-av-",
            _ => ""
        };

    public static string Get(WinRARDictionarySize value)
        => value switch {
            WinRARDictionarySize.D64Kb => "-md64k",
            WinRARDictionarySize.D128Kb => "-md128k",
            WinRARDictionarySize.D256Kb => "md256k",
            WinRARDictionarySize.D512Kb => "-md512k",
            WinRARDictionarySize.D1024Kb => "-md1024k",
            WinRARDictionarySize.D2048Kb => "-md2048k",
            WinRARDictionarySize.D4096Kb => "-md4096k",
            WinRARDictionarySize.D1MB => "-md1m",
            WinRARDictionarySize.D2MB => "-md2m",
            WinRARDictionarySize.D4MB => "-md4m",
            WinRARDictionarySize.D8MB => "-md8m",
            WinRARDictionarySize.D16MB => "-md16m",
            WinRARDictionarySize.D32MB => "-md32m",
            WinRARDictionarySize.D64MB => "-md62m",
            WinRARDictionarySize.D128MB => "-md128m",
            WinRARDictionarySize.D256MB => "-md256m",
            WinRARDictionarySize.D512MB => "-md512m",
            WinRARDictionarySize.D1024MB => "-md1024m",
            _ => ""
        };

    public static string Get(WinRAROverwriteMode value)
        => value switch {
            WinRAROverwriteMode.Silent => "-o+",
            WinRAROverwriteMode.Skip => "-o-",
            _ => ""
        };
}
