namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands.Builder;
public class WinRARCommandBuilder : IWinRARCommandBuilder,
    IWinRARCompressionArgumentsBuilder,
    IWinRARExtractionArgumentBuilder,
    IWinRARCommentArgumentsBuilder,
    IWinRARUpdateArgumentsBuilder,
    IWinRARRepairArgumentsBuilder,
    IWinRARDeleteArgumentsBuilder {


    private WinRARCommand command;


    public IWinRARCommentArgumentsBuilder CreateCommentCommand() {
        return this;
    }

    public IWinRARCompressionArgumentsBuilder CreateCompressionCommand() {
        throw new NotImplementedException();
    }

    public IWinRARDeleteArgumentsBuilder CreateDeleteCommand() {
        throw new NotImplementedException();
    }

    public IWinRARExtractionArgumentBuilder CreateExtractionCommand() {
        throw new NotImplementedException();
    }

    public IWinRARRepairArgumentsBuilder CreateRepairCommand() {
        throw new NotImplementedException();
    }

    public IWinRARUpdateArgumentsBuilder CreateUpdateCommand() {
        throw new NotImplementedException();
    }

    public IWinRARCommentArgumentsBuilder UseFile(FileSnapshot snapshot) {
        throw new NotImplementedException();
    }

    public WinRARCommand Build() {
        throw new NotImplementedException();
    }
}


