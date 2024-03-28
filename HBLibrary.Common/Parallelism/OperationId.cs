using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Parallelism;
public readonly struct OperationId<TKey>(TKey key, string id) : IEquatable<OperationId<TKey>> where TKey : notnull, IEquatable<TKey> {
    public TKey Key { get; } = key;
    public string Id { get; } = id;

    public bool Equals(OperationId<TKey> other) {
        return this.Key.Equals(other.Key) && this.Id == other.Id;
    }

    public override bool Equals(object? obj) {
        return obj is OperationId<TKey> oId && Equals(oId);
    }

    public override int GetHashCode() {
        return HBHashCode.Combine(Key, Id);
    }

    public override string ToString() {
        return $"({Key}, {Id})";
    }
}