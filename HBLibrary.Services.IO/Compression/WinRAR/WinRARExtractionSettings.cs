using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Compression.WinRAR {
    public class WinRARExtractionSettings {
        public string Password { get; set; }
        public WinRARUpdateMode UpdateMode { get; set; } = WinRARUpdateMode.ExtractAndReplace;
        public WinRAROverrideMode OverrideMode { get; set; } = WinRAROverrideMode.Silent;
    }

    public enum WinRARUpdateMode {
        ExtractAndReplace,
        ExtractAndOverride,
        ReplaceOnlyExisting
    }

    public enum WinRAROverrideMode {
        Silent,
        Skip,
        Rename
    }
}
