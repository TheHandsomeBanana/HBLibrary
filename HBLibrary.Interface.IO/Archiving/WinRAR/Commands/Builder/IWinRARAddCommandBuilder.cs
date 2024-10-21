namespace HBLibrary.Interface.IO.Archiving.WinRAR.Commands.Builder;
public interface IWinRARAddCommandBuilder : IWinRARFileEntryCommandBuilder<IWinRARAddCommandBuilder> {
    public IWinRARAddCommandBuilder SetAuthenticityVerification(AuthenticityVerification authenticityVerification); // -av | -av-
    public IWinRARAddCommandBuilder DisableReadConfiguration(); // -cfg-
    public IWinRARAddCommandBuilder IgnoreEmptyDirectories(); // -ed
    public IWinRARAddCommandBuilder FreshenFiles(); // -f
    public IWinRARAddCommandBuilder SetPassword(string password, bool encryptHeaders); // -p | -hp
    public IWinRARAddCommandBuilder LockArchive(); // -k
    public IWinRARAddCommandBuilder KeepBrokenExtractedFiles(); // -kb
    public IWinRARAddCommandBuilder SetCompressionLevel(WinRARCompressionLevel compressionLevel); // -m<0..5>
    public IWinRARAddCommandBuilder SetDictionarySize(WinRARDictionarySize dictionarySize); // -md<size>
    public IWinRARAddCommandBuilder UseMultipleThreads(int threadCount); // -mt<threads>
    public IWinRARAddCommandBuilder SetOverwriteMode(WinRAROverwriteMode overwriteMode); // -o- | -o+
    public IWinRARAddCommandBuilder RecurseSubdirectories(); // -r
    public IWinRARAddCommandBuilder AddDataRecoveryRecord(int size); // -rr[%]
    public IWinRARAddCommandBuilder CreateRecoveryVolumes(WinRARRecoveryVolume recoveryVolume); // -rv[%]
    public IWinRARAddCommandBuilder DeleteOriginalFiles(); // -sdel
    public IWinRARAddCommandBuilder TestArchiveIntegrity(); // -t
    public IWinRARAddCommandBuilder CreateVolumes(WinRARVolumeSize volumeSize); // -v<size>[K|M|G]
    public IWinRARAddCommandBuilder FormatFilenames(WinRARFileNameFormat format); // -cl | -cu
    public IWinRARAddCommandBuilder IgnoreFileAttributes(); // -ai
}