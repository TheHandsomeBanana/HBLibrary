namespace HBLibrary.Services.IO.Archiving.Zip;
public interface IZipExtractor : IExtractor {
    void Extract(FileSnapshot sourceArchive, DirectorySnapshot destinationDirectory, ZipExtractionSettings settings);
}
