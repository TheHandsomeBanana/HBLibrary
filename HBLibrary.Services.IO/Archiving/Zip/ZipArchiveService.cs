namespace HBLibrary.Services.IO.Archiving.Zip;
public class ZipArchiveService : ArchiveServiceBase, IZipArchiveService {
    public IZipCompressor Compressor { get; }
    public IZipExtractor Extractor { get; }

    public ZipArchiveService(IZipCompressor compressor, IZipExtractor extractor) {
        Compressor = compressor;
        Extractor = extractor;
    }

    protected override ICompressor GetCompressor() => Compressor;
    protected override IExtractor GetExtractor() => Extractor;
}
