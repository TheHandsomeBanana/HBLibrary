using HBLibrary.Interface.IO.Archiving.WinRAR.Commands;
using System.Text;

namespace HBLibrary.IO.Archiving.WinRAR.Commands;
public class WinRARAddCommand : WinRARFileEntryCommand {
    public override WinRARCommandName Command => WinRARCommandName.Add; // a
    public WinRARRecoveryVolume? RecoveryVolume { get; init; } = null; // -rv[%]
    public WinRARVolumeSize? VolumeSize { get; init; } = null; // -v<size>[K|M|G]
    public WinRARDataRecoveryRecord? DataRecoveryRecord { get; init; } = null; // -rr[%]
    public AuthenticityVerification? AuthenticityVerification { get; init; } = null; // -av | -av-
    public WinRARCompressionLevel? CompressionLevel { get; init; } // -m<0..5>
    public WinRARDictionarySize? DictionarySize { get; init; } // -md<size>
    public WinRAROverwriteMode? OverwriteMode { get; init; } // -o-, -o+
    public WinRARFileNameFormat? FileNameFormat { get; init; } = null; // -cl, -cu
    public int? ThreadCount { get; init; } = null; // -mt<threads>
    public bool RecurseSubdirectories { get; init; } = true; // -r
    public bool DisableReadConfiguration { get; init; } = false; // -cfg-
    public bool IgnoreEmptyDirectories { get; init; } = true; // -ed
    public bool FreshenFiles { get; init; } = false; // -f
    public bool KeepBrokenExtractedFiles { get; init; } = true; // -kb
    public bool LockArchive { get; init; } = false; // -k
    public bool DeleteOriginalFiles { get; init; } = false; // -sdel
    public bool TestArchiveIntegrity { get; init; } = false; // -t
    public bool IgnoreFileAttributes { get; set; } = false; // -ai

    public override string BuildSwitches() {
        StringBuilder sb = new StringBuilder();
        sb.Append(base.BuildSwitches());

        if (RecoveryVolume.HasValue)
            sb.Append(RecoveryVolume.Value.ToString())
                .Append(' ');

        if (VolumeSize.HasValue)
            sb.Append(VolumeSize.Value.ToString())
                .Append(' ');

        if (AuthenticityVerification.HasValue)
            sb.Append(WinRARNameMapping.Get(AuthenticityVerification.Value))
                .Append(' ');

        if (CompressionLevel.HasValue)
            sb.Append(WinRARNameMapping.Get(CompressionLevel.Value))
                .Append(' ');

        if (DictionarySize.HasValue)
            sb.Append(WinRARNameMapping.Get(DictionarySize.Value))
                .Append(' ');

        if (OverwriteMode.HasValue)
            sb.Append(WinRARNameMapping.Get(OverwriteMode.Value))
                .Append(' ');

        if (ThreadCount.HasValue)
            sb.Append("-mt")
                .Append(ThreadCount.Value)
                .Append(' ');

        if (DataRecoveryRecord.HasValue)
            sb.Append(DataRecoveryRecord.Value.ToString())
                .Append(' ');

        if (FileNameFormat != null)
            sb.Append(WinRARNameMapping.Get(FileNameFormat.Value))
                .Append(' ');

        if (IgnoreFileAttributes)
            sb.Append("-ai ");

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