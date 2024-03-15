namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands.Builder;
public interface IWinRARExtractArgumentBuilder : IWinRARArgumentsBuilder<IWinRARExtractArgumentBuilder> {
    public IWinRARExtractArgumentBuilder SetOverwriteMode(WinRAROverwriteMode overwriteMode); // -o- | -o+
    public IWinRARExtractArgumentBuilder SetPassword(string password); // -p
    public IWinRARExtractArgumentBuilder RecurseSubdirectories(); // -r
    public IWinRARExtractArgumentBuilder AppendArchiveNameToDestination(); // -ad
    public IWinRARExtractArgumentBuilder IgnoreEmptyDirectories(); // -ed
    public IWinRARExtractArgumentBuilder KeepBrokenFiles(); // -kb
    public IWinRARExtractArgumentBuilder IncludeFiles(IEnumerable<string> files); // -n
    public IWinRARExtractArgumentBuilder IncludeFiles(string pattern); // -n
    public IWinRARExtractArgumentBuilder ExcludeFiles(IEnumerable<string> files); // -x
    public IWinRARExtractArgumentBuilder ExcludeFiles(string pattern); // -x
}
