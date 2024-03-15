using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands;
public class WinRARAddCommand : WinRARFileHandlingCommand {
    public override WinRARCommandName Command => WinRARCommandName.Add; // a
    public WinRARPassword? Password { get; init; } = null; // -h, -hp
    public WinRARRecoveryVolume? RecoveryVolume { get; init; } = null; // -rv[%]
    public WinRARVolumeSize? VolumeSize { get; init; } = null; // -v<size>[K|M|G]
    public AuthenticityVerification? AuthenticityVerification { get; init; } = null; // -av | -av-
    public WinRARCompressionLevel CompressionLevel { get; init; } = WinRARCompressionLevel.Normal; // -m<0..5>
    public WinRARDictionarySize DictionarySize { get; init; } = WinRARDictionarySize.Md32m; // -md<size>
    public WinRAROverwriteMode OverwriteMode { get; init; } = WinRAROverwriteMode.Skip; // -o- | -o+
    public int? ThreadCount { get; init; } = null; // -mt<threads>
    public int? DataRecoveryRecord { get; init; } = null; // -rr[%]
    public bool RecurseSubdirectories { get; init; } = true; // -r
    public bool DisableReadConfiguration { get; init; } = false; // -cfg-
    public bool IgnoreEmptyDirectories { get; init; } = true; // -ed
    public bool FreshenFiles { get; init; } = false; // -f
    public bool KeepBrokenExtractedFiles { get; init; } = true; // -kb
    public bool LockArchive { get; init; } = false; // -k
    public bool DeleteOriginalFiles { get; init; } = false; // -sdel
    public bool TestArchiveIntegrity { get; init; } = false; // -t

    public override string BuildContextArguments() {
        StringBuilder sb = new StringBuilder();
        if(Password.HasValue)
            sb.Append(Password.Value.ToString())
                .Append(' ');

        if (VolumeSize.HasValue)
            sb.Append(VolumeSize.Value.ToString())
                .Append(' ');

        
        return sb.ToString();
    }
}

public enum AuthenticityVerification {
    Enabled,
    Disabled,
}


