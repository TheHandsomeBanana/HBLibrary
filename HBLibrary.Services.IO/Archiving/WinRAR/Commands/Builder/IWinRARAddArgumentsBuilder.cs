namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands.Builder;
public interface IWinRARAddArgumentsBuilder : IWinRARArgumentsBuilder<IWinRARAddArgumentsBuilder> {
    public IWinRARAddArgumentsBuilder SetAuthenticityVerification(AuthenticityVerification authenticityVerification); // -av | -av-
    public IWinRARAddArgumentsBuilder DisableReadConfiguration(); // -cfg-
    public IWinRARAddArgumentsBuilder IgnoreEmptyDirectories(); // -ed
    public IWinRARAddArgumentsBuilder FreshenFiles(); // -f
    public IWinRARAddArgumentsBuilder SetPassword(string password, bool encryptHeaders); // -p | -hp
    public IWinRARAddArgumentsBuilder LockArchive(); // -k
    public IWinRARAddArgumentsBuilder KeepBrokenExtractedFiles(); // -kb
    public IWinRARAddArgumentsBuilder SetCompressionLevel(WinRARCompressionLevel compressionLevel); // -m<0..5>
    public IWinRARAddArgumentsBuilder SetDictionarySize(WinRARDictionarySize dictionarySize); // -md<size>
    public IWinRARAddArgumentsBuilder UseMultipleThreads(int threadCount); // -mt<threads>
    public IWinRARAddArgumentsBuilder SetOverwriteMode(WinRAROverwriteMode overwriteMode); // -o- | -o+
    public IWinRARAddArgumentsBuilder RecurseSubdirectories(); // -r
    public IWinRARAddArgumentsBuilder AddDataRecoveryRecord(int size); // -rr[%]
    public IWinRARAddArgumentsBuilder CreateRecoveryVolumes(WinRARRecoveryVolume recoveryVolume); // -rv[%]
    public IWinRARAddArgumentsBuilder DeleteOriginalFiles(); // -sdel
    public IWinRARAddArgumentsBuilder TestArchiveIntegrity(); // -t
    public IWinRARAddArgumentsBuilder CreateVolumes(WinRARVolumeSize volumeSize); // -v<size>[K|M|G]
}