using System.Collections.Immutable;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands.Builder;
public interface IWinRARArgumentsBuilder<TArgumentsBuilder> {
    TArgumentsBuilder ConvertFilenamesLowercase(); // -cl
    TArgumentsBuilder ConvertFilenamesUppercase(); // -cu
    TArgumentsBuilder IgnoreFileAttributes(); // -ai
    TArgumentsBuilder SetProcessPriority(int priority, int waitTimeMs); // -ri[priority][:wait]
    ImmutableArray<string> Build();
}
