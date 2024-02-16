using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Operations; 
public class IOOperationErrorResponse : IOOperationResponse {
    public IOOperationErrorResponse(string error) {
        Success = false;
        ErrorMessage = error;
    }
}
