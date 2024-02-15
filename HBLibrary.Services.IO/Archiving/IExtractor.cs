using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving;
public interface IExtractor {
    void Extract(FileSnapshot sourceArchive, DirectorySnapshot destinationDirectory);
}
