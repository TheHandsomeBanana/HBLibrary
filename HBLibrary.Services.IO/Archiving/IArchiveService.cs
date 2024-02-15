using HBLibrary.Services.IO.Compression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving;
public interface IArchiveService {
    ICompressor Compressor { get; }
    IExtractor Extractor { get; }

    void CompressArchive(Archive contents);
    void DeleteArchive(FileSnapshot file);
    void UpdateArchive(FileSnapshot file, Archive contents);
    void ExtractArchive(FileSnapshot file);
}
