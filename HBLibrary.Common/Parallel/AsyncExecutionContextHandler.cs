using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Parallelism;
public abstract class AsyncExecutionContextHandler<TContext, TKey> where TKey : notnull, IEquatable<TKey> {
    private readonly ConcurrentDictionary<OperationId<TKey>, bool> contextExecutionTracker = [];

    public async Task ExecuteActionAsync(TKey key, Func<TContext, Task> action) {
        TContext context = await GetContextAsync(key);
        await action(context);
    }

    public async Task<TReturn> ExecuteFunctionAsync<TReturn>(TKey key, Func<TContext, Task<TReturn>> function) {
        TContext context = await GetContextAsync(key);
        return await function(context);
    }

    public async Task ExecuteActionSingleAsync(TKey key, string operationId, Func<TContext, Task> action) {
        OperationId<TKey> oId = new OperationId<TKey>(key, operationId);
        if (!contextExecutionTracker.TryAdd(oId, true))
            throw new InvalidOperationException("Action already running.");

        try {
            TContext context = await GetContextAsync(key);
            await action(context);
        }
        finally {
            contextExecutionTracker.TryRemove(oId, out _);
        }
    }

    public async Task<TReturn> ExecuteFunctionSingleAsync<TReturn>(TKey key, string operationId, Func<TContext, Task<TReturn>> function) {
        OperationId<TKey> oId = new OperationId<TKey>(key, operationId);
        if (!contextExecutionTracker.TryAdd(oId, true))
            throw new InvalidOperationException("Action already running.");

        try {
            TContext context = await GetContextAsync(key);
            return await function(context);
        }
        finally {
            contextExecutionTracker.TryRemove(oId, out _);
        }   
    }

    public abstract Task<TContext> GetContextAsync(TKey key);
}



