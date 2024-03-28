using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands.Builder;
internal class WinRARAddCommandBuilder : IWinRARAddCommandBuilder {
    public IWinRARAddCommandBuilder AddDataRecoveryRecord(int size) {
        throw new NotImplementedException();
    }

    public IWinRARAddCommandBuilder AddDirectory(DirectorySnapshot directory) {
        throw new NotImplementedException();
    }

    public IWinRARAddCommandBuilder AddFile(FileSnapshot file) {
        throw new NotImplementedException();
    }

    public IWinRARAddCommandBuilder AddPattern(string pattern) {
        throw new NotImplementedException();
    }

    public string Build() {
        throw new NotImplementedException();
    }

    public IWinRARAddCommandBuilder CreateRecoveryVolumes(WinRARRecoveryVolume recoveryVolume) {
        throw new NotImplementedException();
    }

    public IWinRARAddCommandBuilder CreateVolumes(WinRARVolumeSize volumeSize) {
        throw new NotImplementedException();
    }

    public IWinRARAddCommandBuilder DeleteOriginalFiles() {
        throw new NotImplementedException();
    }

    public IWinRARAddCommandBuilder DisableReadConfiguration() {
        throw new NotImplementedException();
    }

    public IWinRARAddCommandBuilder FormatFilenames(WinRARFileNameFormat format) {
        throw new NotImplementedException();
    }

    public IWinRARAddCommandBuilder FreshenFiles() {
        throw new NotImplementedException();
    }

    public IWinRARAddCommandBuilder IgnoreEmptyDirectories() {
        throw new NotImplementedException();
    }

    public IWinRARAddCommandBuilder IgnoreFileAttributes() {
        throw new NotImplementedException();
    }

    public IWinRARAddCommandBuilder KeepBrokenExtractedFiles() {
        throw new NotImplementedException();
    }

    public IWinRARAddCommandBuilder LockArchive() {
        throw new NotImplementedException();
    }

    public IWinRARAddCommandBuilder RecurseSubdirectories() {
        throw new NotImplementedException();
    }

    public IWinRARAddCommandBuilder SetAuthenticityVerification(AuthenticityVerification authenticityVerification) {
        throw new NotImplementedException();
    }

    public IWinRARAddCommandBuilder SetCompressionLevel(WinRARCompressionLevel compressionLevel) {
        throw new NotImplementedException();
    }

    public IWinRARAddCommandBuilder SetDictionarySize(WinRARDictionarySize dictionarySize) {
        throw new NotImplementedException();
    }

    public IWinRARAddCommandBuilder SetOverwriteMode(WinRAROverwriteMode overwriteMode) {
        throw new NotImplementedException();
    }

    public IWinRARAddCommandBuilder SetPassword(string password, bool encryptHeaders) {
        throw new NotImplementedException();
    }

    public IWinRARAddCommandBuilder SetPassword(WinRARPassword password) {
        throw new NotImplementedException();
    }

    public IWinRARAddCommandBuilder SetProcessPriority(WinRARProcessPriority priority) {
        throw new NotImplementedException();
    }

    public IWinRARAddCommandBuilder SetTargetArchive(ValidPath targetArchive) {
        throw new NotImplementedException();
    }

    public IWinRARAddCommandBuilder TestArchiveIntegrity() {
        throw new NotImplementedException();
    }

    public IWinRARAddCommandBuilder UseMultipleThreads(int threadCount) {
        throw new NotImplementedException();
    }
}
