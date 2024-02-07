using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Operations;
public class ReadFileOperationResponse : FileOperationResponse {
    public byte[] Result { get; init; } = [];
    public string? ResultString { get; init; }

    public override string GetStringResult() {
        throw new NotImplementedException();
    }
}
