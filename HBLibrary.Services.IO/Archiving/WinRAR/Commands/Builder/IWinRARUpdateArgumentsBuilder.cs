namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands.Builder;
public interface IWinRARUpdateArgumentsBuilder : IWinRARArgumentsBuilder<IWinRARUpdateArgumentsBuilder> {
    public IWinRARUpdateArgumentsBuilder SetOverwriteMode(WinRAROverwriteMode overwriteMode); // -o- | -o+
    public IWinRARUpdateArgumentsBuilder SetPassword(string password); // -p
    public IWinRARUpdateArgumentsBuilder RecurseSubdirectories(); // -r
    public IWinRARUpdateArgumentsBuilder SetCompressionLevel(WinRARCompressionLevel compressionLevel); // -m<0..5>
    public IWinRARUpdateArgumentsBuilder OnlyUpdateNewerThan(DateTime dateTime); // -tn<date/time>
    public IWinRARUpdateArgumentsBuilder OnlyUpdateOlderThan(DateTime dateTime); // -to<date/time>
    public IWinRARUpdateArgumentsBuilder FormatFilenames(WinRARFileNameFormat format); // -cl | -cu
    public IWinRARUpdateArgumentsBuilder IgnoreFileAttributes(); // -ai

}
