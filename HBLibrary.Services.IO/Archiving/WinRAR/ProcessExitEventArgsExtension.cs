using HBLibrary.Common.Process;
using System;

namespace HBLibrary.Services.IO.Archiving.WinRAR;
public static class ProcessExitEventArgsExtension {
    public static string GetDescription(this ProcessExitEventArgs eventArgs)
        => GetDescription(eventArgs.ExitCode);

    public static string GetDescription(int exitCode)
        => exitCode switch {
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
            _ => $"Unknown exit code: {exitCode}"
        };
}
