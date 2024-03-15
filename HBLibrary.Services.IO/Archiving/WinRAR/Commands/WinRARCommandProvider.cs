using HBLibrary.Services.IO.Archiving.WinRAR.Commands.Builder;
using HBLibrary.Services.IO.Archiving.WinRAR.ConfigModels;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands;
public class WinRARCommandProvider : IWinRARCommandProvider {
    public WinRARCommand CreateCommand<TArgumentsBuilder>(WinRARCommandName command, Action<TArgumentsBuilder> builder) where TArgumentsBuilder : IWinRARArgumentsBuilder<TArgumentsBuilder> {
        throw new NotImplementedException();
    }

    public WinRARCommand CreateCommentCommand(Action<IWinRARCommentArgumentsBuilder> commentArgumentsBuilder) {
        throw new NotImplementedException();
    }

    public WinRARCommand CreateCompressionCommand(Archive archive, Action<IWinRARAddArgumentsBuilder> compressionArgumentsBuilder) {
        throw new NotImplementedException();
    }

    public WinRARCommand CreateDeleteCommand(Action<IWinRARDeleteArgumentsBuilder> deleteArgumentsBuilder) {
        throw new NotImplementedException();
    }

    public WinRARCommand CreateExtractionCommand(DirectorySnapshot destination, WinRARExtractionMode extractionMode, Action<IWinRARExtractArgumentBuilder> extractionArgumentBuilder) {
        throw new NotImplementedException();
    }

    public WinRARCommand CreateRepairCommand(Action<IWinRARRepairArgumentsBuilder> repairArgumentsBuilder) {
        throw new NotImplementedException();
    }

    public WinRARCommand CreateUpdateCommand(Archive archive, Action<IWinRARUpdateArgumentsBuilder> updateArgumentsBuilder) {
        throw new NotImplementedException();
    }

    public WinRARCommand FromCommandString(string commandString) {
        throw new NotImplementedException();
    }

    public WinRARCommand FromConfiguration(string configuration) {
        throw new NotImplementedException();
    }
}


