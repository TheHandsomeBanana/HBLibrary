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
    public WinRARDataRecoveryRecord? DataRecoveryRecord { get; init; } = null; // -rr[%]
    public AuthenticityVerification? AuthenticityVerification { get; init; } = null; // -av | -av-
    public WinRARCompressionLevel CompressionLevel { get; init; } = WinRARCompressionLevel.Normal; // -m<0..5>
    public WinRARDictionarySize DictionarySize { get; init; } = WinRARDictionarySize.Md32m; // -md<size>
    public WinRAROverwriteMode OverwriteMode { get; init; } = WinRAROverwriteMode.Skip; // -o- | -o+
    public int? ThreadCount { get; init; } = null; // -mt<threads>
    public bool RecurseSubdirectories { get; init; } = true; // -r
    public bool DisableReadConfiguration { get; init; } = false; // -cfg-
    public bool IgnoreEmptyDirectories { get; init; } = true; // -ed
    public bool FreshenFiles { get; init; } = false; // -f
    public bool KeepBrokenExtractedFiles { get; init; } = true; // -kb
    public bool LockArchive { get; init; } = false; // -k
    public bool DeleteOriginalFiles { get; init; } = false; // -sdel
    public bool TestArchiveIntegrity { get; init; } = false; // -t

    public override string BuildSwitches() {
        StringBuilder sb = new StringBuilder();
        sb.Append(base.BuildSwitches());

        if (Password.HasValue)
            sb.Append(Password.Value.ToString())
                .Append(' ');

        if (RecoveryVolume.HasValue)
            sb.Append(RecoveryVolume.Value.ToString())
                .Append(' ');

        if (VolumeSize.HasValue)
            sb.Append(VolumeSize.Value.ToString())
                .Append(' ');

        if (AuthenticityVerification.HasValue)
            switch (AuthenticityVerification.Value) {
                case Commands.AuthenticityVerification.Enabled:
                    sb.Append("-av ");
                    break;
                case Commands.AuthenticityVerification.Disabled:
                    sb.Append("-av- ");
                    break;
            }

        switch (CompressionLevel) {
            case WinRARCompressionLevel.Save:
                sb.Append("-m0 ");
                break;
            case WinRARCompressionLevel.Fastest:
                sb.Append("-m1 ");
                break;
            case WinRARCompressionLevel.Fast:
                sb.Append("-m2 ");
                break;
            case WinRARCompressionLevel.Normal:
                sb.Append("-m3 ");
                break;
            case WinRARCompressionLevel.Good:
                sb.Append("-m4 ");
                break;
            case WinRARCompressionLevel.Best:
                sb.Append("-m5 ");
                break;
        }

        sb.Append(DictionarySize.ToString().ToLower());

        switch (OverwriteMode) {
            case WinRAROverwriteMode.Silent:
                sb.Append("-o+ ");
                break;
            case WinRAROverwriteMode.Skip:
                sb.Append("-o- ");
                break;
        }

        if (ThreadCount.HasValue)
            sb.Append("-mt")
                .Append(ThreadCount.Value)
                .Append(' ');

        if (DataRecoveryRecord.HasValue)
            sb.Append(DataRecoveryRecord.Value.ToString())
                .Append(' ');

        if (RecurseSubdirectories)
            sb.Append("-r ");

        if (DisableReadConfiguration)
            sb.Append("-cfg- ");

        if (IgnoreEmptyDirectories)
            sb.Append("-ed ");

        if (FreshenFiles)
            sb.Append("-f ");

        if (KeepBrokenExtractedFiles)
            sb.Append("-kb ");

        if (LockArchive)
            sb.Append("-k ");

        if (DeleteOriginalFiles)
            sb.Append("-sdel ");

        if (TestArchiveIntegrity)
            sb.Append("-t ");

        return sb.ToString();
    }
}

public enum AuthenticityVerification {
    Enabled,
    Disabled,
}


