using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Compression;
public interface IArchive {
    string Name { get; }
    IEnumerable<string> FileNames { get; }
    IEnumerable<string> DirectoryNames { get; }
}
