using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace HBLibrary.Services.IO.Remote;
public class RemoteFileService : IRemoteFileService {
    private readonly ClientWebSocket client = new ClientWebSocket();
    public Uri ServerUri { get; }
    public RemoteFileService(string serverUri) {
        ServerUri = new Uri(serverUri);
    }

    public Task ConnectAsync(CancellationToken cancellationToken = default) {
        if (client.State != WebSocketState.Open)
            return client.ConnectAsync(ServerUri, cancellationToken);

        return Task.CompletedTask;
    }

    public Task DisconnectAsync(CancellationToken cancellationToken = default) {
        if (client.State == WebSocketState.Open)
            return client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client disconnecting", cancellationToken);

        return Task.CompletedTask;
    }

    


    private bool disposed = false;
    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this); // Prevent finalizer from running
    }

    protected virtual void Dispose(bool disposing) {
        if (!disposed) {
            if (disposing) {
                // Dispose managed state (managed objects).
                client?.Dispose();
            }

            // Free unmanaged resources (unmanaged objects) and override a finalizer below.
            // Set large fields to null.

            disposed = true;
        }
    }

    // Optional: only if unmanaged resources are directly used
    ~RemoteFileService() {
        Dispose(false);
    }
}
