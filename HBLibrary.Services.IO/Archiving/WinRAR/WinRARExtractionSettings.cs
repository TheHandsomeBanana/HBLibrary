using HBLibrary.Services.IO.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR;
public class WinRARExtractionSettings {
    public string? Password { get; set; }
    public WinRARUpdateMode? UpdateMode { get; set; } = null;
    public WinRAROverwriteMode OverwriteMode { get; set; } = WinRAROverwriteMode.Silent;

    public static WinRARExtractionSettings Default => new WinRARExtractionSettings();

    public override string ToString() {
        StringBuilder sb = new StringBuilder();
        sb.Append("x "); // Extract

        if (Password is not null)
            sb.Append("-p" + Password + " ");

        if (UpdateMode.HasValue)
            sb.Append(UpdateModeString + " ");

        return sb.ToString();
    }

    public string UpdateModeString => UpdateMode switch {
        WinRARUpdateMode.ExtractAndReplace or null => "",
        WinRARUpdateMode.ExtractAndUpdate => "-u",
        WinRARUpdateMode.ReplaceOnlyExisting => "-f",
        _ => throw new NotSupportedException(UpdateMode.ToString())
    };

    public string OverwriteModeString => OverwriteMode switch {
        WinRAROverwriteMode.Silent => "-o+",
        WinRAROverwriteMode.Skip => "-o-",
        _ => throw new NotSupportedException(OverwriteMode.ToString())
    };

    
}

public enum WinRARUpdateMode {
    ExtractAndReplace,
    ExtractAndUpdate,
    ReplaceOnlyExisting
}

public enum WinRAROverwriteMode {
    Silent,
    Skip,
}
