namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands.Builder;
public interface IWinRARCommentCommandBuilder : IWinRARCommandBuilder<IWinRARCommentCommandBuilder> {
    IWinRARCommentCommandBuilder UseFile(FileSnapshot snapshot);
}
