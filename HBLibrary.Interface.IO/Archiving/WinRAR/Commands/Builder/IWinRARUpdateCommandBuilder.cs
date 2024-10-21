namespace HBLibrary.Interface.IO.Archiving.WinRAR.Commands.Builder;
public interface IWinRARUpdateCommandBuilder : IWinRARCommandBuilder<IWinRARUpdateCommandBuilder> {
    public IWinRARUpdateCommandBuilder SetOverwriteMode(WinRAROverwriteMode overwriteMode); // -o- | -o+
    public IWinRARUpdateCommandBuilder SetPassword(string password); // -p
    public IWinRARUpdateCommandBuilder RecurseSubdirectories(); // -r
    public IWinRARUpdateCommandBuilder SetCompressionLevel(WinRARCompressionLevel compressionLevel); // -m<0..5>
    public IWinRARUpdateCommandBuilder OnlyUpdateNewerThan(DateTime dateTime); // -tn<date/time>
    public IWinRARUpdateCommandBuilder OnlyUpdateOlderThan(DateTime dateTime); // -to<date/time>
    public IWinRARUpdateCommandBuilder FormatFilenames(WinRARFileNameFormat format); // -cl | -cu
    public IWinRARUpdateCommandBuilder IgnoreFileAttributes(); // -ai

}
