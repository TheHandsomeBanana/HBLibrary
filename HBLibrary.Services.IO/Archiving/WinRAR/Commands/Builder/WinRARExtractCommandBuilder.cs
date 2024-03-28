using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands.Builder;
internal class WinRARExtractCommandBuilder : IWinRARExtractCommandBuilder {
    public IWinRARExtractCommandBuilder AddDirectory(DirectorySnapshot directory) {
        throw new NotImplementedException();
    }

    public IWinRARExtractCommandBuilder AddFile(FileSnapshot file) {
        throw new NotImplementedException();
    }

    public IWinRARExtractCommandBuilder AddPattern(string pattern) {
        throw new NotImplementedException();
    }

    public IWinRARExtractCommandBuilder AppendArchiveNameToDestination() {
        throw new NotImplementedException();
    }

    public string Build() {
        throw new NotImplementedException();
    }

    public IWinRARExtractCommandBuilder IgnoreEmptyDirectories() {
        throw new NotImplementedException();
    }

    public IWinRARExtractCommandBuilder IncludeFiles(IEnumerable<string> files) {
        throw new NotImplementedException();
    }

    public IWinRARExtractCommandBuilder IncludeFiles(string pattern) {
        throw new NotImplementedException();
    }

    public IWinRARExtractCommandBuilder KeepBrokenFiles() {
        throw new NotImplementedException();
    }

    public IWinRARExtractCommandBuilder RecurseSubdirectories() {
        throw new NotImplementedException();
    }

    public IWinRARExtractCommandBuilder SetOverwriteMode(WinRAROverwriteMode overwriteMode) {
        throw new NotImplementedException();
    }

    public IWinRARExtractCommandBuilder SetPassword(string password) {
        throw new NotImplementedException();
    }

    public IWinRARExtractCommandBuilder SetPassword(WinRARPassword password) {
        throw new NotImplementedException();
    }

    public IWinRARExtractCommandBuilder SetProcessPriority(WinRARProcessPriority priority) {
        throw new NotImplementedException();
    }

    public IWinRARExtractCommandBuilder SetTargetArchive(ValidPath targetArchive) {
        throw new NotImplementedException();
    }

    public IWinRARExtractCommandBuilder SetTargetDirectory(DirectorySnapshot target) {
        throw new NotImplementedException();
    }
}
