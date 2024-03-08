using HBLibrary.Services.IO.Archiving.WinRAR.ConfigModels;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands.Builder;
public interface IWinRARCommandBuilder {
    IWinRARCommandBuilder CreateCompressionCommand(Action<IWinRARCompressionArgumentsBuilder> compressionArgumentsBuilder);
    IWinRARCommandBuilder CreateExtractionCommand(WinRARExtractionMode extractionMode, Action<IWinRARExtractionArgumentBuilder> extractionArgumentBuilder);
    IWinRARCommandBuilder CreateCommentCommand(Action<IWinRARCommentArgumentsBuilder> commentArgumentsBuilder);
    IWinRARCommandBuilder CreateUpdateCommand(Action<IWinRARUpdateArgumentsBuilder> updateArgumentsBuilder);
    IWinRARCommandBuilder CreateRepairCommand(Action<IWinRARRepairArgumentsBuilder> repairArgumentsBuilder);
    IWinRARCommandBuilder CreateDeleteCommand(Action<IWinRARDeleteArgumentsBuilder> deleteArgumentsBuilder);
    WinRARCommand Build();
    WinRARCommand FromConfiguration(WinRARCommandConfiguration configuration);
    WinRARCommand FromArgumentString(string argumentString);
}
