namespace HBLibrary.Services.IO.Archiving.WinRAR.Obsolete;
public class WinRARArchiveService : ArchiveServiceBase, IArchiveService<IWinRARCompressor, IWinRARExtractor>
{
    public IWinRARCompressor Compressor { get; }
    public IWinRARExtractor Extractor { get; }

    public WinRARArchiveService(IWinRARCompressor compressor, IWinRARExtractor extractor)
    {
        Compressor = compressor;
        Extractor = extractor;
    }



    public void UpdateArchive(FileSnapshot file, Archive contents)
    {
        throw new NotImplementedException();
    }

    protected override ICompressor GetCompressor() => Compressor;
    protected override IExtractor GetExtractor() => Extractor;
}
