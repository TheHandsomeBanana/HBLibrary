
namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands.Builder;
public class WinRARCommandBuilder : IWinRARCommandBuilder {


    private WinRARCommand command;

    public WinRARCommand Build() {
        throw new NotImplementedException();
    }

    public IWinRARCommandBuilder CreateCommentCommand(Action<IWinRARCommentArgumentsBuilder> commentArgumentsBuilder) {
        throw new NotImplementedException();
    }

    public IWinRARCommandBuilder CreateCompressionCommand(Archive archive, Action<IWinRARCompressionArgumentsBuilder> compressionArgumentsBuilder) {
        throw new NotImplementedException();
    }

    public IWinRARCommandBuilder CreateDeleteCommand(Action<IWinRARDeleteArgumentsBuilder> deleteArgumentsBuilder) {
        throw new NotImplementedException();
    }

    public IWinRARCommandBuilder CreateExtractionCommand(DirectorySnapshot destination, WinRARExtractionMode extractionMode, Action<IWinRARExtractionArgumentBuilder> extractionArgumentBuilder) {
        throw new NotImplementedException();
    }

    public IWinRARCommandBuilder CreateRepairCommand(Action<IWinRARRepairArgumentsBuilder> repairArgumentsBuilder) {
        throw new NotImplementedException();
    }

    public IWinRARCommandBuilder CreateUpdateCommand(Archive archive, Action<IWinRARUpdateArgumentsBuilder> updateArgumentsBuilder) {
        throw new NotImplementedException();
    }

    public WinRARCommand FromCommandString(string commandString) {
        throw new NotImplementedException();
    }

    public WinRARCommand FromConfiguration(string configuration) {
        throw new NotImplementedException();
    }
}


