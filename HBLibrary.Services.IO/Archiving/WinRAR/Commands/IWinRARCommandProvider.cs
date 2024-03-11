using HBLibrary.Services.IO.Archiving.WinRAR.Commands.Builder;
using HBLibrary.Services.IO.Archiving.WinRAR.ConfigModels;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands;
public interface IWinRARCommandProvider {
    WinRARCommand CreateCommand<TArgumentsBuilder>(WinRARCommandName command, Action<TArgumentsBuilder> builder) where TArgumentsBuilder : IWinRARArgumentsBuilder<TArgumentsBuilder>;
    WinRARCommand CreateCompressionCommand(Archive archive, Action<IWinRARCompressionArgumentsBuilder> compressionArgumentsBuilder);
    WinRARCommand CreateExtractionCommand(DirectorySnapshot destination, WinRARExtractionMode extractionMode, Action<IWinRARExtractionArgumentBuilder> extractionArgumentBuilder);
    WinRARCommand CreateCommentCommand(Action<IWinRARCommentArgumentsBuilder> commentArgumentsBuilder);
    WinRARCommand CreateUpdateCommand(Archive archive, Action<IWinRARUpdateArgumentsBuilder> updateArgumentsBuilder);
    WinRARCommand CreateRepairCommand(Action<IWinRARRepairArgumentsBuilder> repairArgumentsBuilder);
    WinRARCommand CreateDeleteCommand(Action<IWinRARDeleteArgumentsBuilder> deleteArgumentsBuilder);
    WinRARCommand FromConfiguration(string configuration);
    WinRARCommand FromCommandString(string commandString);
}
