using HBLibrary.Services.IO.Archiving.WinRAR.ConfigModels;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands.Builder;
public interface IWinRARCommandBuilder {
    IWinRARCommandBuilder CreateCompressionCommand(Archive archive, Action<IWinRARCompressionArgumentsBuilder> compressionArgumentsBuilder);
    IWinRARCommandBuilder CreateExtractionCommand(DirectorySnapshot destination, WinRARExtractionMode extractionMode, Action<IWinRARExtractionArgumentBuilder> extractionArgumentBuilder);
    IWinRARCommandBuilder CreateCommentCommand(Action<IWinRARCommentArgumentsBuilder> commentArgumentsBuilder);
    IWinRARCommandBuilder CreateUpdateCommand(Archive archive, Action<IWinRARUpdateArgumentsBuilder> updateArgumentsBuilder);
    IWinRARCommandBuilder CreateRepairCommand(Action<IWinRARRepairArgumentsBuilder> repairArgumentsBuilder);
    IWinRARCommandBuilder CreateDeleteCommand(Action<IWinRARDeleteArgumentsBuilder> deleteArgumentsBuilder);
    WinRARCommand Build();
    WinRARCommand FromConfiguration(string configuration);
    WinRARCommand FromCommandString(string commandString);
}
