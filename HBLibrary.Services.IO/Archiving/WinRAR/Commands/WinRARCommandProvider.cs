using HBLibrary.Services.IO.Archiving.WinRAR.Commands.Builder;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands;
public class WinRARCommandProvider : IWinRARCommandProvider {
    public IWinRARCommand CreateCommand<TCommandBuilder>(WinRARCommandName command, Func<TCommandBuilder, string> builder) where TCommandBuilder : IWinRARCommandBuilder<TCommandBuilder> {
        TCommandBuilder commandBuilder = Activator.CreateInstance<TCommandBuilder>();
        string argumentString = builder.Invoke(commandBuilder);
        return new PrebuiltWinRARCommand(command, argumentString);
    }

    public IWinRARCommand CreateCommentCommand(Func<IWinRARCommentCommandBuilder, string> commentArgumentsBuilder) {
        return new PrebuiltWinRARCommand(WinRARCommandName.Comment, commentArgumentsBuilder.Invoke(new WinRARCommentCommandBuilder()));
    }

    public IWinRARCommand CreateAddCommand(Func<IWinRARAddCommandBuilder, string> addArgumentsBuilder) {
        return new PrebuiltWinRARCommand(WinRARCommandName.Add, addArgumentsBuilder.Invoke(new WinRARAddCommandBuilder()));
    }

    public IWinRARCommand CreateDeleteCommand(Func<IWinRARDeleteCommandBuilder, string> deleteArgumentsBuilder) {
        return new PrebuiltWinRARCommand(WinRARCommandName.Delete, deleteArgumentsBuilder.Invoke(new WinRARDeleteCommandBuilder()));
    }

    public IWinRARCommand CreateExtractionCommand(WinRARExtractionMode extractionMode, Func<IWinRARExtractCommandBuilder, string> extractionArgumentBuilder) {
        WinRARCommandName extractCommand = extractionMode == WinRARExtractionMode.FullPaths ? WinRARCommandName.ExtractFull : WinRARCommandName.Extract;
        return new PrebuiltWinRARCommand(extractCommand, extractionArgumentBuilder.Invoke(new WinRARExtractCommandBuilder()));
    }

    public IWinRARCommand CreateRepairCommand(Func<IWinRARRepairCommandBuilder, string> repairArgumentsBuilder) {
        return new PrebuiltWinRARCommand(WinRARCommandName.Repair, repairArgumentsBuilder.Invoke(new WinRARRepairCommandBuilder()));
    }

    public IWinRARCommand CreateUpdateCommand(Func<IWinRARUpdateCommandBuilder, string> updateArgumentsBuilder) {
        return new PrebuiltWinRARCommand(WinRARCommandName.Update, updateArgumentsBuilder.Invoke(new WinRARUpdateCommandBuilder()));
    }

    public IWinRARCommand FromCommandString(string commandString) {
        throw new NotImplementedException();
    }

    public IWinRARCommand FromConfiguration(string configuration) {
        throw new NotImplementedException();
    }
}


