using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HBLibrary.Services.IO.Compression;

namespace HBLibrary.Services.IO.Archiving.Zip;
public interface IZipCompressor : ICompressor {
    void Compress(Archive archive, ZipCompressionSettings settings);
}

