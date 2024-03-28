using System.Collections.Immutable;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands.Builder;
public interface IWinRARCommandBuilder<TArgumentsBuilder> {
    TArgumentsBuilder SetTargetArchive(ValidPath targetArchive);
    TArgumentsBuilder SetPassword(WinRARPassword password); // -h, -hp
    TArgumentsBuilder SetProcessPriority(WinRARProcessPriority priority); // -ri[priority][:wait]
    string Build();
}

public interface IWinRARFileEntryCommandBuilder<TArgumentsBuilder> : IWinRARCommandBuilder<TArgumentsBuilder> {
    TArgumentsBuilder AddFile(FileSnapshot file);
    TArgumentsBuilder AddDirectory(DirectorySnapshot directory);
    TArgumentsBuilder AddPattern(string pattern);
}
