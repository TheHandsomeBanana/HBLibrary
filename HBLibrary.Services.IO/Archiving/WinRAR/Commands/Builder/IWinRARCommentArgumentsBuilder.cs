namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands.Builder;
public interface IWinRARCommentArgumentsBuilder : IWinRARArgumentsBuilder<IWinRARCommentArgumentsBuilder> {
    IWinRARCommentArgumentsBuilder UseFile(FileSnapshot snapshot);
}
