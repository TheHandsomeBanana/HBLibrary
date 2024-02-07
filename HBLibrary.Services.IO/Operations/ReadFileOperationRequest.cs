using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Operations;
public class ReadFileOperationRequest : FileOperationRequest {
    public override bool CanAsync => true;
    public FileAccess Access { get; init; } = FileAccess.Read;
    public FileShare Share { get; init; } = FileShare.None;
}
