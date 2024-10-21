namespace HBLibrary.Interface.IO.Archiving.WinRAR.Commands.Builder;
public interface IWinRARCommentCommandBuilder : IWinRARCommandBuilder<IWinRARCommentCommandBuilder> {
    IWinRARCommentCommandBuilder UseFile(FileSnapshot snapshot);
}
