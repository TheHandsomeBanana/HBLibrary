using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands;
public class WinRARAddCommand : WinRARCommand {
    public override WinRARCommandName Command => WinRARCommandName.Add;
    public WinRARPassword? Password { get; init; } = null;
    public WinRARRecoveryVolume? RecoveryVolume { get; init; } = null;
    public WinRARVolumeSize? VolumeSize { get; init; } = null;
    public AuthenticityVerification? AuthenticityVerification { get; init; } = null;
    public WinRARCompressionLevel CompressionLevel { get; init; } = WinRARCompressionLevel.Normal;
    public WinRARDictionarySize DictionarySize { get; init; } = WinRARDictionarySize.Md32m;
    public WinRAROverwriteMode OverwriteMode { get; init; } = WinRAROverwriteMode.Skip;
    public int? ThreadCount { get; init; } = null;
    public int? DataRecoveryRecord { get; init; } = null;
    public bool RecurseSubdirectories { get; init; } = true;
    public bool DisableReadConfiguration { get; init; } = false;
    public bool IgnoreEmptyDirectories { get; init; } = true;
    public bool FreshenFiles { get; init; } = false;
    public bool KeepBrokenExtractedFiles { get; init; } = true;
    public bool LockArchive { get; init; } = false;
    public bool DeleteOriginalFiles { get; init; } = false;
    public bool TestArchiveIntegrity { get; init; } = false;
}

public enum AuthenticityVerification {
    Enabled,
    Disabled,
}
