using HBLibrary.Common.Process;
using HBLibrary.Services.IO.Archiving.WinRAR.ConfigModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR;
public class WinRARExtractor : WinRARArchiverBase, IWinRARExtractor {
    

    public void Extract(FileSnapshot sourceArchive, DirectorySnapshot destinationDirectory, WinRARExtractionSettings settings) {
        throw new NotImplementedException();
    }

    public void Extract(FileSnapshot sourceArchive, DirectorySnapshot destinationDirectory) {
        Extract(sourceArchive, destinationDirectory, WinRARExtractionSettings.Default);
    }

    public Task ExtractAsync(FileSnapshot sourceArchive, DirectorySnapshot destinationDirectory, WinRARExtractionSettings settings, CancellationToken token = default) {
        throw new NotImplementedException();
    }
}
