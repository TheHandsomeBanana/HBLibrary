using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.Zip;
public class ZipArchiveService : ArchiveServiceBase, IArchiveService<IZipCompressor, IZipExtractor> {
    public IZipCompressor Compressor { get; }
    public IZipExtractor Extractor { get; }

    public ZipArchiveService(IZipCompressor compressor, IZipExtractor extractor) {
        Compressor = compressor;
        Extractor = extractor;
    }

    

    public void UpdateArchive(FileSnapshot file, Archive contents) {
        throw new NotImplementedException();
    }

    protected override ICompressor GetCompressor() => Compressor;
    protected override IExtractor GetExtractor() => Extractor;
}
