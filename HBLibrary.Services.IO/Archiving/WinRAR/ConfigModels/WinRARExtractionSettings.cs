using HBLibrary.Services.IO.Archiving.WinRAR;
using HBLibrary.Services.IO.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR.ConfigModels;
public class WinRARExtractionSettings
{
    public string? Password { get; set; }
    public WinRARExecutableMode ExecutableMode { get; set; } = WinRARExecutableMode.RAR;
    public WinRARUpdateMode? UpdateMode { get; set; } = null;
    public WinRAROverwriteMode OverwriteMode { get; set; } = WinRAROverwriteMode.Silent;

    public static WinRARExtractionSettings Default => new WinRARExtractionSettings();

    public override string ToString()
    {
        Validate();

        StringBuilder sb = new StringBuilder();
        sb.Append("x "); // Extract

        if (Password is not null)
            sb.Append("-p" + Password + " ");

        if (UpdateMode.HasValue)
            sb.Append(UpdateModeString + " ");

        return sb.ToString();
    }

    public string UpdateModeString => UpdateMode switch
    {
        WinRARUpdateMode.ExtractAndReplace or null => "",
        WinRARUpdateMode.ExtractAndUpdate => "-u",
        WinRARUpdateMode.ReplaceOnlyExisting => "-f",
        _ => throw new NotSupportedException(UpdateMode.ToString())
    };

    public string OverwriteModeString => OverwriteMode switch
    {
        WinRAROverwriteMode.Silent => "-o+",
        WinRAROverwriteMode.Skip => "-o-",
        WinRAROverwriteMode.Rename => "-or",
        _ => throw new NotSupportedException(OverwriteMode.ToString())
    };

    private void Validate()
    {
        if (ExecutableMode is WinRARExecutableMode.RAR)
        {
            if (UpdateMode.HasValue)
                throw new WinRARException($"{nameof(UpdateMode)} is not supported for the rar executable mode.");

            if (OverwriteMode is WinRAROverwriteMode.Rename)
                throw new WinRARException($"{nameof(OverwriteMode.Rename)} is not supported for the rar executable mode.");
        }
    }
}

public enum WinRARUpdateMode
{
    ExtractAndReplace,
    ExtractAndUpdate,
    ReplaceOnlyExisting
}

public enum WinRAROverwriteMode
{
    Silent,
    Skip,
    Rename
}
