using Microsoft.Graph.Models;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace HBLibrary.Common.Results;
[DebuggerDisplay("State = {ResultState}")]
public readonly struct CollectionResult : IEquatable<CollectionResult> {
    public ResultState ResultState { get; }
    public bool IsSuccess => ResultState == ResultState.Success;
    public bool IsFaulted => ResultState == ResultState.Faulted;
    public ImmutableArray<string> Messages { get; }
    public ImmutableArray<Exception> Exceptions { get; }

    public CollectionResult(ResultState resultState, IEnumerable<string> messages, IEnumerable<Exception> exceptions) {
        ResultState = resultState;
        this.Messages = [.. messages];
        this.Exceptions = [.. exceptions];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CollectionResult Ok() => new CollectionResult(ResultState.Success, [], []);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CollectionResult Fail(string message) => new CollectionResult(ResultState.Faulted, [message], []);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CollectionResult Fail(Exception exception) => new CollectionResult(ResultState.Faulted, [exception.Message], [exception]);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CollectionResult Fail(string message, Exception exception) => new CollectionResult(ResultState.Faulted, [message], [exception]);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CollectionResult Fail(IEnumerable<string> messages) => new CollectionResult(ResultState.Faulted, messages, []);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CollectionResult Fail(IEnumerable<Exception> exceptions) => new CollectionResult(ResultState.Faulted, exceptions.Select(e => e.Message), exceptions);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CollectionResult Fail(IEnumerable<string> messages, IEnumerable<Exception> exceptions) => new CollectionResult(ResultState.Faulted, messages, exceptions);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CollectionResult Fail(IEnumerable<Tuple<string, Exception>> errors) => new CollectionResult(ResultState.Faulted, errors.Select(e => e.Item1), errors.Select(e => e.Item2));


    public IEnumerable<Tuple<string, Exception>> GetMappedErrors() {
        return Messages.Zip(Exceptions, (msg, ex) => new Tuple<string, Exception>(msg, ex));
    }

    [Pure]
    public bool Equals(CollectionResult other) {
        return ResultState == other.ResultState &&
            Messages.SequenceEqual(other.Messages) &&
            Exceptions.SequenceEqual(other.Exceptions);
    }

    [Pure]
    public override bool Equals(object? obj) {
        return obj is CollectionResult res && Equals(res);
    }

    [Pure]
    public override int GetHashCode() {
        return HBHashCode.Combine(ResultState, Messages, Exceptions);
    }

    [Pure]
    public static bool operator ==(CollectionResult left, CollectionResult right) {
        return left.Equals(right);
    }

    [Pure]
    public static bool operator !=(CollectionResult left, CollectionResult right) {
        return !(left == right);
    }

    [Pure]
    public void Deconstruct(out ResultState resultState, out ImmutableArray<string> messages, out ImmutableArray<Exception> exceptions) {
        resultState = ResultState;
        messages = Messages;
        exceptions = Exceptions;
    }

    [Pure]
    public void Deconstruct(out IEnumerable<Tuple<string, Exception>> errors) {
        errors = GetMappedErrors();
    }
}
