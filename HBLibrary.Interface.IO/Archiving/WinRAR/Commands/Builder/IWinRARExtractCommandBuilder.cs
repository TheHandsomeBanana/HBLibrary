namespace HBLibrary.Interface.IO.Archiving.WinRAR.Commands.Builder;
public interface IWinRARExtractCommandBuilder : IWinRARFileEntryCommandBuilder<IWinRARExtractCommandBuilder> {
    public IWinRARExtractCommandBuilder SetOverwriteMode(WinRAROverwriteMode overwriteMode); // -o- | -o+
    public IWinRARExtractCommandBuilder SetPassword(string password); // -p
    public IWinRARExtractCommandBuilder RecurseSubdirectories(); // -r
    public IWinRARExtractCommandBuilder AppendArchiveNameToDestination(); // -ad
    public IWinRARExtractCommandBuilder IgnoreEmptyDirectories(); // -ed
    public IWinRARExtractCommandBuilder KeepBrokenFiles(); // -kb
    public IWinRARExtractCommandBuilder IncludeFiles(IEnumerable<string> files); // -n
    public IWinRARExtractCommandBuilder IncludeFiles(string pattern); // -n
    public IWinRARExtractCommandBuilder SetTargetDirectory(DirectorySnapshot target);
}
