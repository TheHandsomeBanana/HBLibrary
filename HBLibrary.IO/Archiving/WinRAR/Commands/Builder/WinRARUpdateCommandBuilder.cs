﻿using HBLibrary.Interface.IO;
using HBLibrary.Interface.IO.Archiving.WinRAR.Commands;
using HBLibrary.Interface.IO.Archiving.WinRAR.Commands.Builder;

namespace HBLibrary.IO.Archiving.WinRAR.Commands.Builder;
internal class WinRARUpdateCommandBuilder : IWinRARUpdateCommandBuilder {
    public string Build() {
        throw new NotImplementedException();
    }

    public IWinRARUpdateCommandBuilder FormatFilenames(WinRARFileNameFormat format) {
        throw new NotImplementedException();
    }

    public IWinRARUpdateCommandBuilder IgnoreFileAttributes() {
        throw new NotImplementedException();
    }

    public IWinRARUpdateCommandBuilder OnlyUpdateNewerThan(DateTime dateTime) {
        throw new NotImplementedException();
    }

    public IWinRARUpdateCommandBuilder OnlyUpdateOlderThan(DateTime dateTime) {
        throw new NotImplementedException();
    }

    public IWinRARUpdateCommandBuilder RecurseSubdirectories() {
        throw new NotImplementedException();
    }

    public IWinRARUpdateCommandBuilder SetCompressionLevel(WinRARCompressionLevel compressionLevel) {
        throw new NotImplementedException();
    }

    public IWinRARUpdateCommandBuilder SetOverwriteMode(WinRAROverwriteMode overwriteMode) {
        throw new NotImplementedException();
    }

    public IWinRARUpdateCommandBuilder SetPassword(string password) {
        throw new NotImplementedException();
    }

    public IWinRARUpdateCommandBuilder SetPassword(WinRARPassword password) {
        throw new NotImplementedException();
    }

    public IWinRARUpdateCommandBuilder SetProcessPriority(WinRARProcessPriority priority) {
        throw new NotImplementedException();
    }

    public IWinRARUpdateCommandBuilder SetTargetArchive(ValidPath targetArchive) {
        throw new NotImplementedException();
    }
}
