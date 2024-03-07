using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Arguments;
public interface IWinRARCommentArgumentsBuilder : IWinRARArgumentsBuilder, IWinRARCommonArgumentsBuilder<IWinRARCommentArgumentsBuilder> {
    IWinRARCommentArgumentsBuilder UseFile(FileSnapshot snapshot);
}
