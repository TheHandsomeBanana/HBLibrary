namespace HBLibrary.Services.IO.Archiving.WinRAR.Commands.Builder;
public interface IWinRARCommonArgumentsBuilder<TArgumentsBuilder> {
    TArgumentsBuilder ConvertFilenamesLowercase(); // -cl
    TArgumentsBuilder ConvertFilenamesUppercase(); // -cu
    TArgumentsBuilder IgnoreFileAttributes(); // -ai
    TArgumentsBuilder SetProcessPriority(int priority, int waitTimeMs); // -ri[priority][:wait]
}
