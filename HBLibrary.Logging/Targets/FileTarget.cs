using HBLibrary.Interface.Logging;
using HBLibrary.Interface.Logging.Configuration;
using HBLibrary.Interface.Logging.Statements;
using HBLibrary.Interface.Logging.Targets;
using HBLibrary.Logging.Configuration;

namespace HBLibrary.Logging.Targets;
public sealed class FileTarget : TargetWithHeader, ILogTarget, IAsyncLogTarget, IEquatable<FileTarget> {
    public const string TargetName =
         @"                                                                            |" + "\n" +
         @"    _______ __        ______                      __                        |" + "\n" +
         @"   / ____(_) /__     /_  __/___ __________ ____  / /_                       |" + "\n" +
         @"  / /_  / / / _ \     / / / __ `/ ___/ __ `/ _ \/ __/                       |" + "\n" +
         @" / __/ / / /  __/    / / / /_/ / /  / /_/ /  __/ /_                         |" + "\n" +
         @"/_/   /_/_/\___/    /_/  \__,_/_/   \__, /\___/\__/                         |" + "\n" +
         @"                                   /____/                                   |" + "\n" +
         @"____________________________________________________________________________|" + "\n";

    private FileStream? fileStream;
    private StreamWriter? fileStreamWriter;
    private bool keepFileHandle;

    public string FileName { get; }
    public bool UseAsync { get; }
    public bool KeepFileHandle {
        get => keepFileHandle;
        set {
            if (!value) {
                fileStream?.Dispose();
                fileStreamWriter?.Dispose();
            }
            else {
                fileStream = InitStream(FileName, UseAsync);
                fileStreamWriter = new StreamWriter(fileStream);
            }

            keepFileHandle = value;
        }
    }
    public LogLevel? LevelThreshold { get; }

    public FileTarget(string fileName, LogLevel? minLevel = null, bool useAsync = false, bool keepFileHandle = true) {
        FileName = fileName;
        LevelThreshold = minLevel;
        UseAsync = useAsync;
        KeepFileHandle = keepFileHandle;

        if (keepFileHandle) {
            fileStreamWriter!.Write(Logo);
            fileStreamWriter.WriteLine(TargetName);
        }
        else {
            using (FileStream fs = InitStream(fileName, false)) {
                using (StreamWriter sw = new StreamWriter(fs)) {
                    sw.Write(Logo);
                    sw.WriteLine(TargetName);
                }
            }
        }
    }

    public void WriteLog(LogStatement log, LogDisplayFormat format = LogDisplayFormat.Full) {
        if (keepFileHandle) {
            fileStreamWriter!.WriteLine(log.Format(format) + "\n");
            return;
        }

        using (FileStream fs = InitStream(FileName, false)) {
            using (StreamWriter sw = new StreamWriter(fs))
                sw.WriteLine(log.Format(format) + "\n");
        }
    }

    public Task WriteLogAsync(LogStatement log, LogDisplayFormat format = LogDisplayFormat.Full) {
        if (keepFileHandle)
            return fileStreamWriter!.WriteLineAsync(log.Format(format) + "\n");

        using (FileStream fs = new FileStream(FileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 4096, true)) {
            using (StreamWriter sw = new StreamWriter(fs))
                return sw.WriteLineAsync(log.Format(format) + "\n");
        }
    }

    public void Dispose() {
        fileStreamWriter?.Dispose();
        fileStream?.Dispose();
    }

    public bool Equals(FileTarget? other) {
        return other?.fileStream?.Name == fileStream?.Name;
    }

    public override bool Equals(object? obj) {
        return obj is FileTarget ft && Equals(ft);
    }

    public override int GetHashCode() {
        return fileStream?.Name.GetHashCode() ?? 0;
    }

    public static bool operator ==(FileTarget a, FileTarget b) => a.Equals(b);
    public static bool operator !=(FileTarget a, FileTarget b) => !(a == b);

    private static FileStream InitStream(string fileName, bool useAsync)
        => new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 4096, useAsync);
}
