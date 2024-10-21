using HBLibrary.Interface.IO.Archiving.WinRAR.Commands.Builder;

namespace HBLibrary.Interface.IO.Archiving.WinRAR.Commands;
public interface IWinRARCommandProvider {
    IWinRARCommand CreateCommand<TCommandBuilder>(WinRARCommandName command, Func<TCommandBuilder, string> builder) where TCommandBuilder : IWinRARCommandBuilder<TCommandBuilder>;
    IWinRARCommand CreateAddCommand(Func<IWinRARAddCommandBuilder, string> addArgumentsBuilder);
    IWinRARCommand CreateExtractionCommand(WinRARExtractionMode extractionMode, Func<IWinRARExtractCommandBuilder, string> extractionArgumentBuilder);
    IWinRARCommand CreateCommentCommand(Func<IWinRARCommentCommandBuilder, string> commentArgumentsBuilder);
    IWinRARCommand CreateUpdateCommand(Func<IWinRARUpdateCommandBuilder, string> updateArgumentsBuilder);
    IWinRARCommand CreateRepairCommand(Func<IWinRARRepairCommandBuilder, string> repairArgumentsBuilder);
    IWinRARCommand CreateDeleteCommand(Func<IWinRARDeleteCommandBuilder, string> deleteArgumentsBuilder);
    IWinRARCommand FromConfiguration(string configuration);
    IWinRARCommand FromCommandString(string commandString);
}


