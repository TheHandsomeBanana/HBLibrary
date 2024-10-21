namespace HBLibrary.Core.Extensions {
    public static class StreamExtensions {
        public static byte[] Read(this Stream s) {
            return s.Read(s.Length);
        }

        public static byte[] Read(this Stream s, long length) {
            byte[] buffer = new byte[length];

#if NET5_0_OR_GREATER
            s.Read(buffer);
#elif NET472_OR_GREATER

            int numBytesToRead = buffer.Length;
            int numBytesRead = 0;

            while (numBytesToRead > 0) {
                // Read may return anything from 0 to numBytesToRead.
                int n = s.Read(buffer, numBytesRead, numBytesToRead);

                // Break when the end of the file is reached.
                if (n == 0)
                    break;

                numBytesRead += n;
                numBytesToRead -= n;
            }
#endif
            return buffer;
        }

        public static void Write(this Stream s, byte[] buffer) {
            s.Write(buffer, 0, buffer.Length);
        }

        public static async Task<byte[]> ReadAsync(this Stream s, CancellationToken cancellationToken = default) {
            return await s.ReadAsync(s.Length, cancellationToken);
        }

        public static async Task<byte[]> ReadAsync(this Stream s, long length, CancellationToken cancellationToken = default) {
            byte[] buffer = new byte[length];

#if NET5_0_OR_GREATER
            await s.ReadAsync(buffer, cancellationToken);
#elif NET472_OR_GREATER
            int numBytesToRead = buffer.Length;
            int numBytesRead = 0;

            while (!cancellationToken.IsCancellationRequested && numBytesToRead > 0) {
                // Read may return anything from 0 to numBytesToRead.
                int n = await s.ReadAsync(buffer, numBytesRead, numBytesToRead, cancellationToken);

                // Break when the end of the file is reached.
                if (n == 0)
                    break;

                numBytesRead += n;
                numBytesToRead -= n;
            }
#endif

            return buffer;
        }

        public static async Task WriteAsync(this Stream s, byte[] buffer, CancellationToken cancellationToken = default) {
#if NET472_OR_GREATER
            await s.WriteAsync(buffer, 0, buffer.Length);
#elif NET5_0_OR_GREATER
            await s.WriteAsync(buffer, cancellationToken);
#endif
        }

        public static void ResetPosition(this Stream s) {
            s.Position = 0;
        }
    }
}
