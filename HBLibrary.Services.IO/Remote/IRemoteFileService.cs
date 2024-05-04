using HBLibrary.Services.IO.Obsolete.Operations.File;

namespace HBLibrary.Services.IO.Remote;
public interface IRemoteFileService : IDisposable {
    Uri ServerUri { get; }
    Task ConnectAsync(CancellationToken cancellationToken = default);
    Task DisconnectAsync(CancellationToken cancellationToken = default);
    Task<FileOperationResponse?> ExecuteAsync(FileOperationRequest operation, CancellationToken token = default);

}
