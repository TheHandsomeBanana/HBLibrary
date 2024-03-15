using HBLibrary.Services.IO.Archiving.WinRAR.Commands.Builder;
using HBLibrary.Services.IO.Archiving.WinRAR.ConfigModels;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands;
public interface IWinRARCommandProvider {
    IWinRARCommand CreateCommand<TArgumentsBuilder>(WinRARCommandName command, Action<TArgumentsBuilder> builder) where TArgumentsBuilder : IWinRARArgumentsBuilder<TArgumentsBuilder>;
    IWinRARCommand CreateCompressionCommand(Archive archive, Action<IWinRARAddArgumentsBuilder> compressionArgumentsBuilder);
    IWinRARCommand CreateExtractionCommand(DirectorySnapshot destination, WinRARExtractionMode extractionMode, Action<IWinRARExtractArgumentBuilder> extractionArgumentBuilder);
    IWinRARCommand CreateCommentCommand(Action<IWinRARCommentArgumentsBuilder> commentArgumentsBuilder);
    IWinRARCommand CreateUpdateCommand(Archive archive, Action<IWinRARUpdateArgumentsBuilder> updateArgumentsBuilder);
    IWinRARCommand CreateRepairCommand(Action<IWinRARRepairArgumentsBuilder> repairArgumentsBuilder);
    IWinRARCommand CreateDeleteCommand(Action<IWinRARDeleteArgumentsBuilder> deleteArgumentsBuilder);
    IWinRARCommand FromConfiguration(string configuration);
    IWinRARCommand FromCommandString(string commandString);
}
