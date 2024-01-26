using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Compression {
    public interface ICompressionService {
        void CompressFile(string sourceFile, string destinationArchive);
        void CompressDirectory(string sourceDirectory, string destinationArchive);
        void Extract(string sourceArchive, string destinationDirectory);
    }
}
