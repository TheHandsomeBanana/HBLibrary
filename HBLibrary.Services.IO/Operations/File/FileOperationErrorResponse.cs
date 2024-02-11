using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Operations.File;
public class FileOperationErrorResponse : FileOperationResponse {

    public FileOperationErrorResponse(string error) {
        Success = false;
        ErrorMessage = error;
    }

    public override string GetStringResult() {
        return base.ToString();
    }
}
