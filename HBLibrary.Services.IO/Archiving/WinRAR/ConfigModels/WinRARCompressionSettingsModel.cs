using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HBLibrary.Services.IO.Archiving.WinRAR;

namespace HBLibrary.Services.IO.Archiving.WinRAR.ConfigModels;
[Serializable]
public class WinRARCompressionSettingsModel {
    public WinRARCompressionMethod? CompressionMethod { get; set; }
    public WinRARDictionarySize? DictionarySize { get; set; }
    public WinRARExecutionMode? ExecutionMode { get; set; }
    public WinRARVolumeSize? VolumeSize { get; set; }
    public string? Password { get; set; }
    public bool ProtectAgainstChanges { get; set; }
    public string? RecoveryRecordPercentage { get; set; }
}
