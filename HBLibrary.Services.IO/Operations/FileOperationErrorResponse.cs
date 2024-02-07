using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Operations;
public class FileOperationErrorResponse : FileOperationResponse {

    public FileOperationErrorResponse(string error) {
        Success = false;
        Message = error;
    }

    public override string GetStringResult() {
        return base.ToString();
    }
}
