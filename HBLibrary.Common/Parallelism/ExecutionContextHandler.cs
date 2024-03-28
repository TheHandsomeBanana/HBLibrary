using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Parallelism;
public abstract class ExecutionContextHandler<TContext, TKey> where TKey : notnull, IEquatable<TKey> {
    private readonly ConcurrentDictionary<OperationId<TKey>, bool> contextExecutionTracker = [];

    public void ExecuteAction(TKey key, Action<TContext> action) {
        TContext context = GetContext(key);
        action(context);
    }

    public TReturn ExecuteFunction<TReturn>(TKey key, Func<TContext, TReturn> function) {
        TContext context = GetContext(key);
        return function(context);
    }

    public void ExecuteActionSingle(TKey key, string operationId, Action<TContext> action) {
        OperationId<TKey> oId = new OperationId<TKey>(key, operationId);
        if (!contextExecutionTracker.TryAdd(oId, true))
            throw new InvalidOperationException("Action already running.");

        try {
            TContext context = GetContext(key);
            action(context);
        }
        finally {
            contextExecutionTracker.TryRemove(oId, out _);
        }
    }

    public TReturn ExecuteFunctionSingle<TReturn>(TKey key, string operationId, Func<TContext, TReturn> function) {
        OperationId<TKey> oId = new OperationId<TKey>(key, operationId);
        if (!contextExecutionTracker.TryAdd(oId, true))
            throw new InvalidOperationException("Action already running.");

        try {
            TContext context = GetContext(key);
            return function(context);
        }
        finally {
            contextExecutionTracker.TryRemove(oId, out _);
        }
    }

    public abstract TContext GetContext(TKey key);
}
