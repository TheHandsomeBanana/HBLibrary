﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands;
public interface IWinRARCommand {
    public WinRARCommandName Command { get; }
    public string ToCommandString();

    
}
