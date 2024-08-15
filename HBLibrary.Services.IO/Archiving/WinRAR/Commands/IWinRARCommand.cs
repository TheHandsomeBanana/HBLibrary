namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands;
public interface IWinRARCommand {
    public WinRARCommandName Command { get; }
    public string ToCommandString();


}
