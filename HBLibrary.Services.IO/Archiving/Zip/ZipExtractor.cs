using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.Zip;
public class ZipExtractor : IZipExtractor {
    public void Extract(FileSnapshot sourceArchive, DirectorySnapshot destinationDirectory, ZipExtractionSettings settings) {
        using (ZipFile zip = ZipFile.Read(sourceArchive.FullPath)) {
            if (settings.Password != null)
                zip.Password = settings.Password;

            zip.ExtractAll(destinationDirectory.FullPath, settings.ExtractExistingFileAction);
        }
    }

    public void Extract(FileSnapshot sourceArchive, DirectorySnapshot destinationDirectory) {
        Extract(sourceArchive, destinationDirectory, ZipExtractionSettings.Default);
    }
}
