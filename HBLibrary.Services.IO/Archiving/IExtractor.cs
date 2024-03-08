namespace HBLibrary.Services.IO.Archiving;
public interface IExtractor {
    void Extract(FileSnapshot sourceArchive, DirectorySnapshot destinationDirectory);
}
