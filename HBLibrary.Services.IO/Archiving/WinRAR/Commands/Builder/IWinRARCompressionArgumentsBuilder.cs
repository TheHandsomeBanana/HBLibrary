namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands.Builder;
public interface IWinRARCompressionArgumentsBuilder : IWinRARCommonArgumentsBuilder<IWinRARCompressionArgumentsBuilder> {
    public IWinRARCompressionArgumentsBuilder AuthenticityVerification(bool enabled); // -av | -av-
    public IWinRARCompressionArgumentsBuilder DisableReadConfiguration(); // -cfg-
    public IWinRARCompressionArgumentsBuilder IgnoreEmptyDirectories(); // -ed
    public IWinRARCompressionArgumentsBuilder FreshenFiles(); // -f
    public IWinRARCompressionArgumentsBuilder SetPassword(string password, bool encryptHeaders); // -p | -hp
    public IWinRARCompressionArgumentsBuilder LockArchive(); // -k
    public IWinRARCompressionArgumentsBuilder KeepBrokenExtractedFiles(); // -kb
    public IWinRARCompressionArgumentsBuilder SetCompressionLevel(WinRARCompressionLevel compressionLevel); // -m<0..5>
    public IWinRARCompressionArgumentsBuilder SetDictionarySize(WinRARDictionarySize dictionarySize); // -md<size>
    public IWinRARCompressionArgumentsBuilder UseMultipleThreads(int threadCount); // -mt<threads>
    public IWinRARCompressionArgumentsBuilder SetOverwriteMode(WinRAROverwriteMode overwriteMode); // -o- | -o+
    public IWinRARCompressionArgumentsBuilder IncludeSubdirectories(); // -r
    public IWinRARCompressionArgumentsBuilder AddDataRecoveryRecord(int size); // -rr[%]
    public IWinRARCompressionArgumentsBuilder CreateRecoveryVolumes(int size, WinRARSizeType sizeType); // -rv[%]
    public IWinRARCompressionArgumentsBuilder DeleteOriginalFiles(); // -sdel
    public IWinRARCompressionArgumentsBuilder TestArchiveIntegrity(); // -t
    public IWinRARCompressionArgumentsBuilder CreateVolumes(WinRARVolumeSize volumeSize); // -v<size>[K|M|G]
}