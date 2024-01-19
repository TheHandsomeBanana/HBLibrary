using HBLibrary.NetFramework.Services.Logging.Configuration;
using HBLibrary.NetFramework.Services.Logging.Statements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging.Targets {
    public class FileTarget : ILogTarget, IAsyncLogTarget, IEquatable<FileTarget> {
        public LogLevel LevelThreshold { get; set; }
        public FileStream FileStream { get; }
        public StreamWriter FileStreamWriter { get; }
        public FileTarget(string fileName, LogLevel minLevel, bool useAsync) {
            FileStream = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.Write, 4096, useAsync);
            FileStreamWriter = new StreamWriter(FileStream);
            LevelThreshold = minLevel;
        }

        public void WriteLog(LogStatement log, LogDisplayFormat format) {
            FileStreamWriter.WriteLine(log.Format(format));
        }

        public Task WriteLogAsync(LogStatement log, LogDisplayFormat format) {
            return FileStreamWriter.WriteLineAsync(log.Format(format));
        }

        public void Dispose() {
            FileStreamWriter.Dispose();
            FileStream.Dispose();
        }

        public bool Equals(FileTarget other) {
            return other.FileStream.Name == FileStream.Name;
        }

        public override bool Equals(object obj) {
            return obj is FileTarget ft && Equals(ft);
        }

        public override int GetHashCode() {
            return FileStream.Name.GetHashCode();
        }

        public static bool operator ==(FileTarget a, FileTarget b) => a.Equals(b);
        public static bool operator !=(FileTarget a, FileTarget b) => !(a == b);
    }
}
