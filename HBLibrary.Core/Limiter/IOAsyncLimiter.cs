using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Core.Limiter;
public static class IOAsyncLimiter {
    public const int DefaultFileConcurrencyLimit = 6;
    public static SemaphoreSlim FileSemaphore { get; private set; }

    static IOAsyncLimiter() {
        FileSemaphore = new SemaphoreSlim(DefaultFileConcurrencyLimit);
    }

    public static void CreateFileSemaphore(int concurrencyLimit) {
        FileSemaphore.Dispose();
        FileSemaphore = new SemaphoreSlim(concurrencyLimit);
    }

}
