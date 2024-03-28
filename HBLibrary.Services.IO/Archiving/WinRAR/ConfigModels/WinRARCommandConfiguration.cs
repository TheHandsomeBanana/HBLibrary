using HBLibrary.Services.IO.Archiving.WinRAR.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR.ConfigModels;
[Serializable]
public abstract class WinRARCommandConfiguration {
    public WinRARCommandName CommandName { get; set; }

}
