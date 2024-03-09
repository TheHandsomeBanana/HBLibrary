using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.Zip; 
public interface IZipArchiveService {
    IZipCompressor Compressor { get; }
    IZipExtractor Extractor { get; }
    public void CompressArchive(Archive contents);
    public void DeleteArchive(FileSnapshot file);
    public void ExtractArchive(FileSnapshot file, DirectorySnapshot directory);
}
