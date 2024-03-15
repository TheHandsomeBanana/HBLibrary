﻿namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands.Builder;
public interface IWinRARUpdateArgumentsBuilder : IWinRARArgumentsBuilder<IWinRARUpdateArgumentsBuilder> {
    public IWinRARUpdateArgumentsBuilder SetOverwriteMode(WinRAROverwriteMode overwriteMode); // -o- | -o+
    public IWinRARUpdateArgumentsBuilder SetPassword(string password); // -p
    public IWinRARUpdateArgumentsBuilder RecurseSubdirectories(); // -r
    public IWinRARAddArgumentsBuilder SetCompressionLevel(WinRARCompressionLevel compressionLevel); // -m<0..5>
    public IWinRARCommentArgumentsBuilder OnlyUpdateNewerThan(DateTime dateTime); // -tn<date/time>
    public IWinRARCommentArgumentsBuilder OnlyUpdateOlderThan(DateTime dateTime); // -to<date/time>

}
