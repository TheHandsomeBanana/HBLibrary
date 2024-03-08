namespace HBLibrary.Services.IO.Archiving.Zip;
public interface IZipCompressor : ICompressor {
    void Compress(Archive archive, ZipCompressionSettings settings);
}

