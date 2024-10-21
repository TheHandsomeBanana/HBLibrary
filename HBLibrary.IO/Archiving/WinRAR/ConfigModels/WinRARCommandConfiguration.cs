using HBLibrary.Interface.IO.Archiving.WinRAR.Commands;

namespace HBLibrary.IO.Archiving.WinRAR.ConfigModels;
[Serializable]
public abstract class WinRARCommandConfiguration {
    public WinRARCommandName CommandName { get; set; }

}
