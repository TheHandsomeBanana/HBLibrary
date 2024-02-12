using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Operations.File;
public class CopyFileOperationRequest : FileOperationRequest {
    public override bool CanAsync => true;
    public required string TargetFile { get; set; }
    public required CopyConflictAction ConflictAction { get; set; }
    public required IFileEntryService FileEntryService { get; set; }

}
