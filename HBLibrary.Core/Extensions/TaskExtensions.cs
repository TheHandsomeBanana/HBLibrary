using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Core.Extensions;

public static class TaskExtensions {
    public static void FireAndForget(this Task task, Action<Exception>? onException = null) {
        task.ContinueWith(t => {
            if (t.IsFaulted && t.Exception is not null) {
                onException?.Invoke(t.Exception);
            }

        }, TaskContinuationOptions.OnlyOnFaulted);
    }
}
