using HBLibrary.Common.RegularExpressions;
using HBLibrary.Services.IO.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR;
// https://documentation.help/WinRAR/HELPCommandLineSyntax.htm
public class WinRARCompressionSettings
{
    public string ArchiveFormatExtension => ArchiveFormat switch
    {
        WinRARArchiveFormat.RAR or
        WinRARArchiveFormat.RAR4 => ".rar",
        WinRARArchiveFormat.ZIP => ".zip",
        _ => throw new NotSupportedException(ArchiveFormat.ToString())
    };

    public WinRARExecutableMode ExecutableMode { get; init; } = WinRARExecutableMode.RAR;
    public WinRARArchiveFormat ArchiveFormat { get; init; } = WinRARArchiveFormat.RAR;
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
    public string? RecoveryRecordPercentage
    {
        get => recoveryRecordPercentage;
        set
        {
            if (value is null)
            {
                recoveryRecordPercentage = null;
                return;
            }

            if (!RegexCollection.SimplePercentageValue.Match(value).Success)
                throw new CompressionException($"{value} does not match the percentage value pattern.");

            recoveryRecordPercentage = value;
        }
    }

    public static WinRARCompressionSettings Default => new WinRARCompressionSettings();
    public string SetExtension(string path)
    {
        if (path.EndsWith(ArchiveFormatExtension))
            return path;

        return path + ArchiveFormatExtension;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    /// <exception cref="WinRARException"></exception>
    public override string ToString()
    {
        Validate();

        StringBuilder sb = new StringBuilder();
        if (ExecutionMode == WinRARExecutionMode.Background)
            sb.Append(ExecutionModeString + " ");

        sb.Append("a -r "); // a = compress, -r = recursive

        if (ExecutableMode == WinRARExecutableMode.WinRAR)
            sb.Append(ArchiveFormatString + " ");

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
    public string GetArgumentString()
    {
        return ToString();
    }

    public string ArchiveFormatString => ArchiveFormat switch
    {
        WinRARArchiveFormat.RAR or
        WinRARArchiveFormat.RAR4 => "-af rar",
        WinRARArchiveFormat.ZIP => "-af zip",
        _ => throw new NotSupportedException()
    };

    public string CompressionMethodString => CompressionMethod switch
    {
        WinRARCompressionMethod.Save => "-m0",
        WinRARCompressionMethod.Fastest => "-m1",
        WinRARCompressionMethod.Fast => "-m2",
        WinRARCompressionMethod.Normal => "-m3",
        WinRARCompressionMethod.Good => "-m4",
        WinRARCompressionMethod.Best => "-m5",
        _ => throw new NotSupportedException(CompressionMethod.ToString())
    };

    public string DictionarySizeString => "-" + DictionarySize.ToString().ToLower();

    public readonly static IReadOnlyDictionary<WinRARArchiveFormat, WinRARDictionarySize[]> ArchiveFormatDictionarySizeMapping
        = new Dictionary<WinRARArchiveFormat, WinRARDictionarySize[]>() {
                { WinRARArchiveFormat.ZIP, [WinRARDictionarySize.Md32k] },
                { WinRARArchiveFormat.RAR4, [
                    WinRARDictionarySize.Md64k,
                    WinRARDictionarySize.Md128k,
                    WinRARDictionarySize.Md256k,
                    WinRARDictionarySize.Md512k,
                    WinRARDictionarySize.Md1024k,
                    WinRARDictionarySize.Md2048k,
                    WinRARDictionarySize.Md4096k,
                    ]
                },
                { WinRARArchiveFormat.RAR, [
                    WinRARDictionarySize.Md1m,
                    WinRARDictionarySize.Md2m,
                    WinRARDictionarySize.Md4m,
                    WinRARDictionarySize.Md8m,
                    WinRARDictionarySize.Md16m,
                    WinRARDictionarySize.Md32m,
                    WinRARDictionarySize.Md64m,
                    WinRARDictionarySize.Md128m,
                    WinRARDictionarySize.Md256m,
                    WinRARDictionarySize.Md512m,
                    WinRARDictionarySize.Md1024m,
                    ]
                }
    };

    public string ExecutionModeString => ExecutionMode switch
    {
        WinRARExecutionMode.Foreground => "",
        WinRARExecutionMode.Background => "-ibck",
        _ => throw new NotSupportedException(ExecutionMode.ToString())
    };

    /// <summary>
    /// Validates the settings combination
    /// </summary>
    /// <exception cref="WinRARException"></exception>
    public void Validate()
    {
        if (!ArchiveFormatDictionarySizeMapping[ArchiveFormat].Contains(DictionarySize))
            throw new WinRARException($"Invalid dictionary size {DictionarySize} for given archive format {ArchiveFormat}." +
                $"Valid dictionary sizes are {string.Join(",", ArchiveFormatDictionarySizeMapping[ArchiveFormat])}");

        if (ExecutableMode is WinRARExecutableMode.RAR && ArchiveFormat is WinRARArchiveFormat.ZIP)
            throw new WinRARException($"ZIP archive is not supported when using the RAR executable mode.");
    }
}

public readonly struct WinRARVolumeSize
{
    public WinRARSizeType SizeType { get; }
    public int Size { get; }

    public WinRARVolumeSize(WinRARSizeType sizeType, int size)
    {
        SizeType = sizeType;
        Size = size;
    }


    public static WinRARVolumeSize Email25MB() => new WinRARVolumeSize(WinRARSizeType.MB, 25);
    public static WinRARVolumeSize WebUpload100MB() => new WinRARVolumeSize(WinRARSizeType.MB, 100);
    public static WinRARVolumeSize WebUpload200MB() => new WinRARVolumeSize(WinRARSizeType.MB, 200);

    public override string ToString()
    {
        switch (SizeType)
        {
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


public enum WinRARSizeType
{
    kB,
    MB,
    GB
}

public enum WinRARArchiveFormat
{
    RAR,
    RAR4,
    ZIP
}

public enum WinRARCompressionMethod
{
    Save,
    Fastest,
    Fast,
    Normal,
    Good,
    Best
}

public enum WinRARDictionarySize
{
    Md32k,
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

public enum WinRARExecutionMode
{
    Foreground,
    Background,
}

public enum WinRARExecutableMode
{
    /// <summary>
    /// Utilizes Rar.exe and UnRAR.exe, GUI will not trigger on failure
    /// </summary>
    RAR,
    /// <summary>
    /// Utilizes WinRAR.exe, GUI will trigger on failure
    /// </summary>
    WinRAR
}

