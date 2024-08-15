using HBLibrary.Services.IO.Archiving.WinRAR.Commands;

namespace HBLibrary.Services.IO.Archiving.WinRAR.ConfigModels;
[Serializable]
public abstract class WinRARCommandConfiguration {
    public WinRARCommandName CommandName { get; set; }

}
