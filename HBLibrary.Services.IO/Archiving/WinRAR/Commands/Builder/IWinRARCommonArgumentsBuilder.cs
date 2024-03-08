namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands.Builder;
public interface IWinRARCommonArgumentsBuilder<TArgumentsBuilder> {
    TArgumentsBuilder ConvertFilenamesLowercase();
    TArgumentsBuilder ConvertFilenamesUppercase();
    TArgumentsBuilder IgnoreFileAttributes();
    TArgumentsBuilder SetProcessPriority(int priority, int waitTimeMs);
    TArgumentsBuilder ExcludeItems(IEnumerable<string> items);
}
