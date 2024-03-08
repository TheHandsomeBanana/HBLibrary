using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Arguments;
public interface IWinRARCompressionArgumentsBuilder : IWinRARArgumentsBuilder, IWinRARCommonArgumentsBuilder<IWinRARCompressionArgumentsBuilder> {
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
    public IWinRARCompressionArgumentsBuilder SetOverwriteMode(WinRAROverwriteMode overwriteMode); // -o-, -o+
    public IWinRARCompressionArgumentsBuilder RecurseSubdirectories(); // -r
    public IWinRARCompressionArgumentsBuilder AddDataRecoveryRecord(int size); // -rr[%]
    public IWinRARCompressionArgumentsBuilder CreateRecoveryVolumes(int size, WinRARSizeType sizeType); // -rv[%]

}