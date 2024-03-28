using HBLibrary.Common.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR;
public class WinRARCommandExecutionResult {
    public required string StdOutput { get; init; }
    public required string StdError { get; init; }
    public required int ExitCode { get; init; }
    public string? ExitCodeMessage { get; init; }
    public required bool IsCanceled { get; init; }
    public required DateTime StartTime { get; init; }
    public required DateTime EndTime { get; init; }
    public TimeSpan Duration => EndTime - StartTime;

    internal static string GetDescription(int exitCode)
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
