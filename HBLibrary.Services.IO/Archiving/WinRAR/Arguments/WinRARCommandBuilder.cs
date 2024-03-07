using HBLibrary.Services.IO.Archiving.WinRAR.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Arguments;
public class WinRARCommandBuilder : IWinRARCommandBuilderScope
    IWinRARCompressionArgumentsBuilder, 
    IWinRARExctractionArgumentBuilder,
    IWinRARCommentArgumentsBuilder,
    IWinRARUpdateArgumentsBuilder,
    IWinRARRepairArgumentsBuilder,
    IWinRARDeleteArgumentsBuilder {


    public WinRARCommand Build() {
        throw new NotImplementedException();
    }

    public IWinRARCommentArgumentsBuilder UseFile(FileSnapshot snapshot) {
        throw new NotImplementedException();
    }
}


