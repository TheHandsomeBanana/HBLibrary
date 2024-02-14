using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Compression {
    public interface ICompressor {
        void Compress(string source, string destinationArchive);
        void Extract(string sourceArchive, string destinationDirectory);

        void Compress(IArchive archive);
        void Compress(Func<IArchiveBuilder, IArchive> archiveBuilder);
    }
}
