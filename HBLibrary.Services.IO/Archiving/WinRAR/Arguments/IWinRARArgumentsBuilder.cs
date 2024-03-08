namespace HBLibrary.Services.IO.Archiving.WinRAR.Arguments;
public interface IWinRARArgumentsBuilder {
    WinRARCommand ParseArgumentString(string argumentString);
    WinRARCommand Build();
}