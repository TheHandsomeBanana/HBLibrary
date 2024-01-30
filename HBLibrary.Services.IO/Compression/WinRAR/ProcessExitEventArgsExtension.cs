﻿using HBLibrary.Common.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Compression.WinRAR;
public static class ProcessExitEventArgsExtension {
    public static string GetDescription(this ProcessExitEventArgs eventArgs)
        => eventArgs.ExitCode switch {
        0 => "Successful operation.",
        1 => "Warning. Non-fatal error(s) occurred.",
        2 => "A fatal error occurred.",
        3 => "Invalid checksum. Data is damaged.",
        4 => "Attempt to modify a locked archive.",
        5 => "Write error.",
        6 => "File open error.",
        7 => "Wrong command line option.",
        8 => "Not enough memory.",
        9 => "File create error.",
        10 => "No files matching the specified mask and options were found.",
        11 => "Wrong password.",
        255 => "User break.",
        _ =>$"Unknown exit code: {eventArgs.ExitCode}"
    };
}
