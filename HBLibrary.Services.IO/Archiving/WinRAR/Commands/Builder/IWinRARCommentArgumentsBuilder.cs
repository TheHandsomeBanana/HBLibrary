namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands.Builder;
public interface IWinRARCommentArgumentsBuilder : IWinRARCommonArgumentsBuilder<IWinRARCommentArgumentsBuilder> {
    IWinRARCommentArgumentsBuilder UseFile(FileSnapshot snapshot);
}
