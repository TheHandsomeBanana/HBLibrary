using HBLibrary.Common.Collections;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Parallel;
public class ConcurrentExecutionManager<TKey> where TKey : notnull, IEquatable<TKey> {
    private readonly ConcurrentHashSet<OperationId<TKey>> executions = [];

    public async Task ExecuteAsync(TKey key, string id, Func<Task> action) {
        OperationId<TKey> opId = new OperationId<TKey>(key, id);
        if (executions.Add(opId)) {
            await action();
            executions.TryRemove(opId);
        }
    }

    public async Task<TResult> ExecuteAsync<TResult>(TKey key, string id, Func<Task<TResult>> function) {
        OperationId<TKey> opId = new OperationId<TKey>(key, id);
        if (executions.Add(opId)) {
            TResult result = await function();
            executions.TryRemove(opId);
            return result;
        }

        throw new InvalidOperationException();
    }
}
