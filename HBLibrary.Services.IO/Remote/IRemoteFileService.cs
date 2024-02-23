using HBLibrary.Services.IO.Operations.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Remote;
public interface IRemoteFileService : IDisposable {
    Uri ServerUri { get; }
    Task ConnectAsync(CancellationToken cancellationToken = default);
    Task DisconnectAsync(CancellationToken cancellationToken = default);
    Task<FileOperationResponse?> ExecuteAsync(FileOperationRequest operation, CancellationToken token = default);

}
