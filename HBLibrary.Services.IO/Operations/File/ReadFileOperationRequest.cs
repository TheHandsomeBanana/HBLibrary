using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Operations.File;
public class ReadFileOperationRequest : FileOperationRequest
{
    public override bool CanAsync => true;
    public FileShare Share { get; set; } = FileShare.None;
    public FileAccess Access => FileAccess.Read;
}