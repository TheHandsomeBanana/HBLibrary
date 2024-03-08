namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands.Builder;
public interface IWinRARExtractionArgumentBuilder : IWinRARCommonArgumentsBuilder<IWinRARExtractionArgumentBuilder> {
    public IWinRARExtractionArgumentBuilder SetOverwriteMode(WinRAROverwriteMode overwriteMode); // -o- | -o+
    public IWinRARExtractionArgumentBuilder SetPassword(string password); // -p
    public IWinRARExtractionArgumentBuilder RecurseSubdirectories(); // -r


}
