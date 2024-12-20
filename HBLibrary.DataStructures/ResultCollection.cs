using HBLibrary.Core;
using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace HBLibrary.DataStructures;
[DebuggerDisplay("State = {resultState}")]
public class ResultCollection : IEquatable<ResultCollection>, IEnumerable<Result>, ICollection<Result>, IReadOnlyCollection<Result>, IReadOnlyList<Result> {
    private ResultState resultState;
    private readonly List<Result> results = [];
    public bool IsSuccess => resultState == ResultState.Success;
    public bool IsFaulted => resultState == ResultState.Faulted;

    public int Count => results.Count;
    public bool IsReadOnly => false;

    public Result this[int index] => results[index];

    private ResultCollection(ResultState resultState, IEnumerable<Result> results) {
        this.resultState = resultState;
        this.results = [.. results];
    }

    public ResultCollection(int size) {
        results = new List<Result>(size);
    }

    public ResultCollection() { }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ResultCollection Ok() => new ResultCollection(ResultState.Success, [Result.Ok()]);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ResultCollection Fail(Exception exception) => new ResultCollection(ResultState.Faulted, [Result.Fail(exception)]);
    public static ResultCollection Create(IEnumerable<Result> results) {
        ImmutableArray<Result> resultsArray = [.. results];
        if (resultsArray.Length == 0) {
            return Ok();
        }

        return new ResultCollection(resultsArray.All(r => r.IsSuccess) ? ResultState.Success : ResultState.Faulted, resultsArray);
    }

    public bool Equals(ResultCollection? other) {
        return resultState == other?.resultState &&
            results.SequenceEqual(other.results);
    }

    public override bool Equals(object? obj) {
        return obj is ResultCollection res && Equals(res);
    }

    public override int GetHashCode() {
        return HBHashCode.Combine(resultState, results);
    }

    public static bool operator ==(ResultCollection left, ResultCollection right) {
        return left.Equals(right);
    }

    public static bool operator !=(ResultCollection left, ResultCollection right) {
        return !(left == right);
    }

    public void Deconstruct(out ResultState resultState, out IReadOnlyCollection<Result> results) {
        resultState = this.resultState;
        results = this.results;
    }

    public IEnumerator<Result> GetEnumerator() {
        foreach (Result res in results) {
            yield return res;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    public void Add(Result item) {
        if (IsSuccess && item.IsFaulted)
            resultState = ResultState.Faulted;

        results.Add(item);
    }

    public void Clear() {
        resultState = ResultState.Success;
        results.Clear();
    }

    public bool Contains(Result item) {
        return results.Contains(item);
    }

    public void CopyTo(Result[] array, int arrayIndex) {
        results.CopyTo(array, arrayIndex);
    }

    public bool Remove(Result item) {
        bool removedSuccess = results.Remove(item);
        if (removedSuccess && item.IsFaulted && results.Any(e => e.IsFaulted)) {
            resultState = ResultState.Faulted;
        }
        return removedSuccess;
    }

    public ImmutableResultCollection ToImmutableResultCollection() {
        return new ImmutableResultCollection(this);
    }
}

[DebuggerDisplay("State = {resultState}")]
public readonly struct ImmutableResultCollection : IEquatable<ImmutableResultCollection>, IEnumerable<Result>, IReadOnlyCollection<Result>, IReadOnlyList<Result> {
    private readonly ResultState resultState;
    private readonly ImmutableArray<Result> results;

    public bool IsSuccess => resultState == ResultState.Success;
    public bool IsFaulted => resultState == ResultState.Faulted;

    public int Count => results.Length;

    public Result this[int index] => results[index];

    public ImmutableResultCollection(ResultCollection results) {
        resultState = results.IsSuccess ? ResultState.Success : ResultState.Faulted;
        this.results = [.. results];
    }

    private ImmutableResultCollection(Result result) {
        results = [result];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ImmutableResultCollection Ok() => new ImmutableResultCollection(Result.Ok());
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ImmutableResultCollection Fail(Exception e) => new ImmutableResultCollection(Result.Fail(e));

    public static implicit operator ImmutableResultCollection(ResultCollection results) => new ImmutableResultCollection(results);


    public IEnumerator<Result> GetEnumerator() {
        foreach (Result res in results) {
            yield return res;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    public bool Equals(ImmutableResultCollection other) {
        return resultState == other.resultState &&
            results.SequenceEqual(other.results);
    }

    public override bool Equals(object? obj) {
        return obj is ImmutableResultCollection res && Equals(res);
    }

    public override int GetHashCode() {
        return HBHashCode.Combine(resultState, results);
    }

    public static bool operator ==(ImmutableResultCollection left, ImmutableResultCollection right) {
        return left.Equals(right);
    }

    public static bool operator !=(ImmutableResultCollection left, ImmutableResultCollection right) {
        return !(left == right);
    }
}