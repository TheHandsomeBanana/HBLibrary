using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR;
public class WinRARArchiveService : IArchiveService<IWinRARCompressor, IWinRARExtractor> {
    public IWinRARCompressor Compressor { get; }
    public IWinRARExtractor Extractor { get; }

    public WinRARArchiveService(IWinRARCompressor compressor, IWinRARExtractor extractor) {
        this.Compressor = compressor;
        this.Extractor = extractor;
    }

    public void CompressArchive(Archive contents) {
        throw new NotImplementedException();
    }

    public void DeleteArchive(FileSnapshot file) {
        throw new NotImplementedException();
    }

    public void ExtractArchive(FileSnapshot file) {
        throw new NotImplementedException();
    }

    public void UpdateArchive(FileSnapshot file, Archive contents) {
        throw new NotImplementedException();
    }
}
