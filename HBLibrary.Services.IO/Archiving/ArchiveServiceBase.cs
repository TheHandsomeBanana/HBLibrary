namespace HBLibrary.Services.IO.Archiving;
public abstract class ArchiveServiceBase {
    public void CompressArchive(Archive contents) {
        GetCompressor().Compress(contents);
    }

    public void DeleteArchive(FileSnapshot file) {
        File.Delete(file.FullPath);
    }

    public void ExtractArchive(FileSnapshot file, DirectorySnapshot directory) {
        GetExtractor().Extract(file, directory);
    }

    protected abstract ICompressor GetCompressor();
    protected abstract IExtractor GetExtractor();
}
