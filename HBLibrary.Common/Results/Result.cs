using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace HBLibrary.Common.Results;
[DebuggerDisplay("State = {ResultState}, Value = {Value}, Error = {Error}")]
public readonly struct Result<TValue, TError> : IEquatable<Result<TValue, TError>>, IEquatable<TValue>  {
    public ResultState ResultState { get; }
    public TValue? Value { get; }
    public TError? Error { get; }

    private Result(ResultState resultState, TValue? value, TError? error) {
        ResultState = resultState;
        Value = value;
        Error = error;
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue, TError> Success(TValue value) => new Result<TValue, TError>(ResultState.Success, value, default);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue, TError> Failure(TError value) => new Result<TValue, TError>(ResultState.Failure, default, value);

    public static implicit operator Result<TValue, TError>(TValue value) => new Result<TValue, TError>(ResultState.Success, value, default);
    public static implicit operator Result<TValue, TError>(TError error) => new Result<TValue, TError>(ResultState.Failure, default, error);

    public void Deconstruct(out ResultState resultState, out TValue? value, out TError? error) {
        resultState = ResultState;
        value = Value;
        error = Error;
    }

    [Pure]
    public R Match<R>(Func<TValue, R> success, Func<TError, R> failure) 
        => ResultState == ResultState.Success 
        ? success(Value!) 
        : failure(Error!);

    [Pure]
    public Task<R> MatchAsync<R>(Func<TValue, Task<R>> success, Func<TError, Task<R>> failure)
        => ResultState == ResultState.Success
        ? success(Value!)
        : failure(Error!);


    public override bool Equals([NotNullWhen(true)] object? obj) {
        return obj is Result<TValue, TError> other && Equals(other) ||
            obj is TValue otherValue && Equals(otherValue);
    }

    public bool Equals(Result<TValue, TError> other) {
       return ResultState == other.ResultState &&
            EqualityComparer<TValue?>.Default.Equals(Value, other.Value) &&
            EqualityComparer<TError?>.Default.Equals(Error, other.Error);
    }

    public bool Equals(TValue? other) {
        return ResultState == ResultState.Success &&
            EqualityComparer<TValue?>.Default.Equals(Value, other);
    }

    public static bool operator ==(Result<TValue, TError> left, Result<TValue, TError> right) {
        return left.Equals(right);
    }

    public static bool operator !=(Result<TValue, TError> left, Result<TValue, TError> right) {
        return !(left == right);
    }

    public static bool operator ==(TValue left, Result<TValue, TError> right) {
        return (left?.Equals(right)).GetValueOrDefault();
    }

    public static bool operator !=(TValue left, Result<TValue, TError> right) {
        return !(left == right);
    }

    public override int GetHashCode() {
        return HBHashCode.Combine(ResultState, Value, Error);
    }
}

[DebuggerDisplay("State = {ResultState}, Value = {Value}, Error = {Error}")]
public readonly struct Result<TValue> : IEquatable<Result<TValue>>, IEquatable<TValue> {
    public ResultState ResultState { get; }
    public TValue? Value { get; }
    public Exception? Error { get; }

    private Result(ResultState resultState, TValue? value, Exception? error) {
        ResultState = resultState;
        Value = value;
        Error = error;
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue> Success(TValue value) => new Result<TValue>(ResultState.Success, value, default);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue> Failure(Exception value) => new Result<TValue>(ResultState.Failure, default, value);


    public static implicit operator Result<TValue>(TValue value) => new Result<TValue>(ResultState.Success, value, default);
    public static implicit operator Result<TValue>(Exception error) => new Result<TValue>(ResultState.Failure, default, error);

    public void Deconstruct(out ResultState resultState, out TValue? value, out Exception? error) {
        resultState = ResultState;
        value = Value;
        error = Error;
    }

    [Pure]
    public R Match<R>(Func<TValue, R> success, Func<Exception, R> failure) 
        => ResultState == ResultState.Success
    ? success(Value!)
    : failure(Error!);

    [Pure]
    public Task<R> MatchAsync<R>(Func<TValue, Task<R>> success, Func<Exception, Task<R>> failure) 
        => ResultState == ResultState.Success
        ? success(Value!)
        : failure(Error!);
    

    public override bool Equals([NotNullWhen(true)] object? obj) {
        return obj is Result<TValue> other && Equals(other) ||
            obj is TValue otherValue && Equals(otherValue);
    }

    public bool Equals(Result<TValue> other) {
        return ResultState == other.ResultState &&
             EqualityComparer<TValue?>.Default.Equals(Value, other.Value) &&
             Error == other.Error;
    }

    public bool Equals(TValue? other) {
        return ResultState == ResultState.Success &&
            EqualityComparer<TValue?>.Default.Equals(Value, other);
    }

    public static bool operator ==(Result<TValue> left, Result<TValue> right) {
        return left.Equals(right);
    }

    public static bool operator !=(Result<TValue> left, Result<TValue> right) {
        return !(left == right);
    }

    public static bool operator ==(TValue left, Result<TValue> right) {
        return (left?.Equals(right)).GetValueOrDefault();
    }

    public static bool operator !=(TValue left, Result<TValue> right) {
        return !(left == right);
    }

    public override int GetHashCode() {
        return HBHashCode.Combine(ResultState, Value, Error);
    }
}

public enum ResultState {
    Success,
    Failure
}