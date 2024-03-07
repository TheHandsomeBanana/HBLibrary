using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Arguments;
public interface IWinRARCommandBuilderScope {
    IWinRARCompressionArgumentsBuilder BeginCompressionCommand();
    IWinRARExctractionArgumentBuilder BeginExtractionCommand();
    IWinRARCommentArgumentsBuilder BeginCommentCommand();
    IWinRARUpdateArgumentsBuilder BeginUpdateCommand();
    IWinRARRepairArgumentsBuilder BeginRepairCommand();
    IWinRARDeleteArgumentsBuilder BeginDeleteCommand();

}
