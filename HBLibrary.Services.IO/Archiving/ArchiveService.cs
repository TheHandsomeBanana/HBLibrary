using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving;
public class ArchiveService : IArchiveService {
    public ICompressor Compressor => throw new NotImplementedException();

    public IExtractor Extractor => throw new NotImplementedException();

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
