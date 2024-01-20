using HBLibrary.NetFramework.Services.Logging.Configuration;
using HBLibrary.NetFramework.Services.Logging.Exceptions;
using HBLibrary.NetFramework.Services.Logging.Statements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging.Targets {
    public class FileTarget : ILogTarget, IAsyncLogTarget, IEquatable<FileTarget> {
        private FileStream fileStream;
        private StreamWriter fileStreamWriter;
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
        public LogLevel LevelThreshold { get; set; }
        
        public FileTarget(string fileName, LogLevel minLevel, bool useAsync = false, bool keepFileHandle = true) {
            this.FileName = fileName;
            this.keepFileHandle = keepFileHandle;
            this.LevelThreshold = minLevel;
            fileStream = InitStream(fileName, useAsync);
            fileStreamWriter = new StreamWriter(fileStream);
            LevelThreshold = minLevel;
        }

        public void WriteLog(LogStatement log, LogDisplayFormat format = LogDisplayFormat.Full) {
            if (keepFileHandle) {
                fileStreamWriter.WriteLine(log.Format(format) + "\n");
                return;
            }

            using (FileStream fs = InitStream(FileName, false)) {
                using (StreamWriter sw = new StreamWriter(fs))
                    sw.WriteLine(log.Format(format) + "\n");
            }
        }

        public Task WriteLogAsync(LogStatement log, LogDisplayFormat format = LogDisplayFormat.Full) {
            if (keepFileHandle)
                return fileStreamWriter.WriteLineAsync(log.Format(format) + "\n");

            using (FileStream fs = new FileStream(FileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 4096, true)) {
                using (StreamWriter sw = new StreamWriter(fs))
                    return sw.WriteLineAsync(log.Format(format) + "\n");
            }
        }

        public void Dispose() {
            fileStreamWriter?.Dispose();
            fileStream?.Dispose();
        }

        public bool Equals(FileTarget other) {
            return other.fileStream.Name == fileStream.Name;
        }

        public override bool Equals(object obj) {
            return obj is FileTarget ft && Equals(ft);
        }

        public override int GetHashCode() {
            return fileStream.Name.GetHashCode();
        }

        public static bool operator ==(FileTarget a, FileTarget b) => a.Equals(b);
        public static bool operator !=(FileTarget a, FileTarget b) => !(a == b);

        private static FileStream InitStream(string fileName, bool useAsync)
            => new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 4096, useAsync);
    }
}
