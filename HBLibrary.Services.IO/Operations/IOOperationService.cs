using HBLibrary.Services.IO.Operations.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Operations;
public static class IOOperationService {
    public static IOOperationResponse Execute(IOOperationRequest request) {
        throw new NotImplementedException();
    }

    public static async Task<IOOperationResponse> ExecuteAsync(IOOperationRequest request) {
        if (request is FileOperationRequest fileRequest)
            return await ExecuteFileOperationAsync(fileRequest);

        throw new NotImplementedException();
    }

    public static FileOperationResponse ExecuteFileOperation(FileOperationRequest operation) {
        return new FileService().Execute(operation);
    }

    public static Task<FileOperationResponse> ExecuteFileOperationAsync(FileOperationRequest operation, CancellationToken token = default) {
        return new FileService().ExecuteAsync(operation);
    }
}
