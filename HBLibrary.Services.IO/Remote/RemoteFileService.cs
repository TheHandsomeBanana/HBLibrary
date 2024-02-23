using HBLibrary.Services.IO.Operations.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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

    public async Task<FileOperationResponse?> ExecuteAsync(FileOperationRequest operation, CancellationToken token = default) {
        await ConnectAsync();

        string json = JsonSerializer.Serialize(operation);
        byte[] buffer = Encoding.UTF8.GetBytes(json);

        await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, token);

        var responseBuffer = new byte[1024 * 4]; // Adjust buffer size as needed
        var response = await client.ReceiveAsync(new ArraySegment<byte>(responseBuffer), token);

        // Deserialize the response into a FileOperationResponse object
        string responseJson = Encoding.UTF8.GetString(responseBuffer, 0, response.Count);
        FileOperationResponse? fileResponse = JsonSerializer.Deserialize<FileOperationResponse>(responseJson);

        return fileResponse;
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
