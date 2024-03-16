using HBLibrary.Services.IO.Archiving.WinRAR.Commands.Builder;
using HBLibrary.Services.IO.Archiving.WinRAR.ConfigModels;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands;
public class WinRARCommandProvider : IWinRARCommandProvider {
    public IWinRARCommand CreateCommand<TArgumentsBuilder>(WinRARCommandName command, Action<TArgumentsBuilder> builder) where TArgumentsBuilder : IWinRARArgumentsBuilder<TArgumentsBuilder> {
        throw new NotImplementedException();
    }

    public IWinRARCommand CreateCommentCommand(Action<IWinRARCommentArgumentsBuilder> commentArgumentsBuilder) {
        throw new NotImplementedException();
    }

    public IWinRARCommand CreateCompressionCommand(Archive archive, Action<IWinRARAddArgumentsBuilder> compressionArgumentsBuilder) {
        throw new NotImplementedException();
    }

    public IWinRARCommand CreateDeleteCommand(Action<IWinRARDeleteArgumentsBuilder> deleteArgumentsBuilder) {
        throw new NotImplementedException();
    }

    public IWinRARCommand CreateExtractionCommand(DirectorySnapshot destination, WinRARExtractionMode extractionMode, Action<IWinRARExtractArgumentBuilder> extractionArgumentBuilder) {
        throw new NotImplementedException();
    }

    public IWinRARCommand CreateRepairCommand(Action<IWinRARRepairArgumentsBuilder> repairArgumentsBuilder) {
        throw new NotImplementedException();
    }

    public IWinRARCommand CreateUpdateCommand(Archive archive, Action<IWinRARUpdateArgumentsBuilder> updateArgumentsBuilder) {
        throw new NotImplementedException();
    }

    public IWinRARCommand FromCommandString(string commandString) {
        throw new NotImplementedException();
    }

    public IWinRARCommand FromConfiguration(string configuration) {
        throw new NotImplementedException();
    }
}


