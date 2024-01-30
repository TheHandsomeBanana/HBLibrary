using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Compression.WinRAR.ConfigModels;
#if NETFRAMEWORK
[Serializable]
#endif
public class WinRARCompressionSettingsModel {
    public WinRARExecutableMode? ExecutableMode { get; set; }
    public WinRARArchiveFormat? ArchiveFormat { get; set; }
    public WinRARCompressionMethod? CompressionMethod { get; set; }
    public WinRARDictionarySize? DictionarySize { get; set; }
    public WinRARExecutionMode? ExecutionMode { get; set; }
    public WinRARVolumeSize? VolumeSize { get; set; }
    public string? Password { get; set; }
    public bool ProtectAgainstChanges { get; set; }
    public string? RecoveryRecordPercentage { get; set; }
}
