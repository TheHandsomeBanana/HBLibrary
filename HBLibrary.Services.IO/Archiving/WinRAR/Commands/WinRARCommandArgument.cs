using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands;
public readonly struct WinRARCommandArgument {
    public string Argument { get; }
    public WinRARCommandArgument(string argument) {
        this.Argument = argument;
    }
}
