using HBLibrary.Services.IO.Archiving.WinRAR.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.WinRAR.Arguments;
public class WinRARCommandBuilder : IWinRARCommandBuilderScope,
    IWinRARCompressionArgumentsBuilder, 
    IWinRARExctractionArgumentBuilder,
    IWinRARCommentArgumentsBuilder,
    IWinRARUpdateArgumentsBuilder,
    IWinRARRepairArgumentsBuilder,
    IWinRARDeleteArgumentsBuilder {


    private WinRARCommand command;


    public IWinRARCommentArgumentsBuilder BeginCommentCommand() {
        return this;
    }

    public IWinRARCompressionArgumentsBuilder BeginCompressionCommand() {
        throw new NotImplementedException();
    }

    public IWinRARDeleteArgumentsBuilder BeginDeleteCommand() {
        throw new NotImplementedException();
    }

    public IWinRARExctractionArgumentBuilder BeginExtractionCommand() {
        throw new NotImplementedException();
    }

    public IWinRARRepairArgumentsBuilder BeginRepairCommand() {
        throw new NotImplementedException();
    }

    public IWinRARUpdateArgumentsBuilder BeginUpdateCommand() {
        throw new NotImplementedException();
    }

    public IWinRARCommentArgumentsBuilder UseFile(FileSnapshot snapshot) {
        throw new NotImplementedException();
    }

    public WinRARCommand Build() {
        throw new NotImplementedException();
    }
}


