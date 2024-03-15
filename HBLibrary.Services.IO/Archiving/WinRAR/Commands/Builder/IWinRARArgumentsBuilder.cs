using System.Collections.Immutable;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands.Builder;
public interface IWinRARArgumentsBuilder<TArgumentsBuilder> {
    TArgumentsBuilder FormatFilenames(WinRARFileNameFormat format); // -cl | -cu
    TArgumentsBuilder IgnoreFileAttributes(); // -ai
    TArgumentsBuilder SetProcessPriority(WinRARProcessPriority priority); // -ri[priority][:wait]
    ImmutableArray<string> Build();
}
