using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Arguments;
public interface IWinRARCommonArgumentsBuilder<TArgumentsBuilder> where TArgumentsBuilder : IWinRARArgumentsBuilder {
    TArgumentsBuilder ConvertFilenamesLowercase();
    TArgumentsBuilder ConvertFilenamesUppercase();
    TArgumentsBuilder IgnoreFileAttributes();
    TArgumentsBuilder SetProcessPriority(int priority, int waitTimeMs);
    TArgumentsBuilder ExcludeItems(IEnumerable<string> items);
}
