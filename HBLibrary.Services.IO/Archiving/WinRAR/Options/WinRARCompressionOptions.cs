using HBLibrary.Common.RegularExpressions;
using HBLibrary.Services.IO.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Options;
// https://documentation.help/WinRAR/HELPCommandLineSyntax.html
public class WinRARCompressionOptions {
    public const string ArchiveFormatExtension = ".rar";
    public FileSnapshot? Comment { get; init; }



    public WinRARCompressionMethod CompressionMethod { get; init; } = WinRARCompressionMethod.Normal;
    public WinRARDictionarySize DictionarySize { get; init; } = WinRARDictionarySize.Md32m;
    public WinRARExecutionMode ExecutionMode { get; init; } = WinRARExecutionMode.Background;
    /// <summary>
    /// Split to volumes
    /// </summary>
    public WinRARVolumeSize? VolumeSize { get; init; }
    public string? Password { get; init; }
    public bool ProtectAgainstChanges { get; init; } = false;

    private string? recoveryRecordPercentage;
    /// <summary>
    /// e.g. '5%' -> adds a recovery record that is 5% of the total archive size
    /// </summary>
    public string? RecoveryRecordPercentage {
        get => recoveryRecordPercentage;
        set {
            if (value is null) {
                recoveryRecordPercentage = null;
                return;
            }

            if (!RegexCollection.SimplePercentageValue.Match(value).Success)
                throw new CompressionException($"{value} does not match the percentage value pattern.");

            recoveryRecordPercentage = value;
        }
    }

    public static WinRARCompressionOptions Default => new WinRARCompressionOptions();
    public string SetExtension(string path) {
        if (path.EndsWith(ArchiveFormatExtension))
            return path;

        return path + ArchiveFormatExtension;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    /// <exception cref="WinRARException"></exception>
    public override string ToString() {

        StringBuilder sb = new StringBuilder();
        if (ExecutionMode == WinRARExecutionMode.Background)
            sb.Append(ExecutionModeString + " ");

        sb.Append("a -r "); // a = compress, -r = recursive

        sb.Append(CompressionMethodString + " ");
        if (ProtectAgainstChanges)
            sb.Append("-k ");

        if (RecoveryRecordPercentage is not null)
            sb.Append("-rr" + RecoveryRecordPercentage + " ");

        if (Password is not null)
            sb.Append("-p" + Password + " ");

        if (VolumeSize.HasValue)
            sb.Append(VolumeSize.Value.ToString());

        sb.Append(DictionarySizeString + " ");

        return sb.ToString();
    }

    /// <summary>
    /// Returns the settings as argument <see cref="string"/> for internal use.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="WinRARException"></exception>
    public string GetArgumentString() {
        return ToString();
    }

    public string CompressionMethodString => CompressionMethod switch {
        WinRARCompressionMethod.Save => "-m0",
        WinRARCompressionMethod.Fastest => "-m1",
        WinRARCompressionMethod.Fast => "-m2",
        WinRARCompressionMethod.Normal => "-m3",
        WinRARCompressionMethod.Good => "-m4",
        WinRARCompressionMethod.Best => "-m5",
        _ => throw new NotSupportedException(CompressionMethod.ToString())
    };

    public string DictionarySizeString => "-" + DictionarySize.ToString().ToLower();

    public string ExecutionModeString => ExecutionMode switch {
        WinRARExecutionMode.Foreground => "",
        WinRARExecutionMode.Background => "-ibck",
        _ => throw new NotSupportedException(ExecutionMode.ToString())
    };
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
    GB
}

public enum WinRARCompressionMethod {
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

public enum WinRARExecutionMode {
    Foreground,
    Background,
}

