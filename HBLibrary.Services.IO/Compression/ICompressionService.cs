using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Compression {
    public interface ICompressionService {
        void Compress(string sourceFile, string destinationArchive);
        void Extract(string sourceArchive, string destinationDirectory);
    }
}
