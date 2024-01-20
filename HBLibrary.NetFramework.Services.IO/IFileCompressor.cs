using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.IO {
    public interface IFileCompressor {
        void CompressFile(string filePath, string compressedFilePath);
    }
}
