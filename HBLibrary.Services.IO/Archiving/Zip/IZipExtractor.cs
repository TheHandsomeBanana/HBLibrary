using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.Zip;
public interface IZipExtractor : IExtractor {
    void Extract(FileSnapshot sourceArchive, DirectorySnapshot destinationDirectory, ZipExtractionSettings settings);
}
