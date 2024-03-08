namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands.Builder;
public interface IWinRARExtractionArgumentBuilder : IWinRARCommonArgumentsBuilder<IWinRARExtractionArgumentBuilder> {
    public IWinRARExtractionArgumentBuilder SetOverwriteMode(WinRAROverwriteMode overwriteMode); // -o- | -o+
    public IWinRARExtractionArgumentBuilder SetPassword(string password); // -p
    public IWinRARExtractionArgumentBuilder RecurseSubdirectories(); // -r
    public IWinRARExtractionArgumentBuilder AppendArchiveNameToDestination(); // -ad
    public IWinRARExtractionArgumentBuilder IgnoreEmptyDirectories(); // -ed
    public IWinRARExtractionArgumentBuilder KeepBrokenFiles(); // -kb
    public IWinRARExtractionArgumentBuilder IncludeFiles(IEnumerable<string> files); // -n
    public IWinRARExtractionArgumentBuilder IncludeFiles(string pattern); // -n
    public IWinRARExtractionArgumentBuilder ExcludeFiles(IEnumerable<string> files); // -x
    public IWinRARExtractionArgumentBuilder ExcludeFiles(string pattern); // -x
}
