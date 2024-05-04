using HBLibrary.Services.IO.Obsolete.Operations.File;

namespace HBLibrary.Services.IO.Obsolete.Operations;
public static class IOOperationService
{
    public static IOOperationResponse Execute(IOOperationRequest request)
    {
        throw new NotImplementedException();
    }

    public static async Task<IOOperationResponse> ExecuteAsync(IOOperationRequest request)
    {
        if (request is FileOperationRequest fileRequest)
            return await ExecuteFileOperationAsync(fileRequest);

        throw new NotImplementedException();
    }

    public static FileOperationResponse ExecuteFileOperation(FileOperationRequest operation)
    {
        return new FileService().Execute(operation);
    }

    public static Task<FileOperationResponse> ExecuteFileOperationAsync(FileOperationRequest operation, CancellationToken token = default)
    {
        return new FileService().ExecuteAsync(operation);
    }
}
