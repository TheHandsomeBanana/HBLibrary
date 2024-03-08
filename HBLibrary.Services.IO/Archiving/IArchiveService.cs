namespace HBLibrary.Services.IO.Archiving;
public interface IArchiveService<TCompressor, TExtractor> where TCompressor : ICompressor where TExtractor : IExtractor {
    TCompressor Compressor { get; }
    TExtractor Extractor { get; }

    void CompressArchive(Archive contents);
    void DeleteArchive(FileSnapshot file);
    void UpdateArchive(FileSnapshot file, Archive contents);
    void ExtractArchive(FileSnapshot file, DirectorySnapshot directory);
}
