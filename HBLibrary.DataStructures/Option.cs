using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.DataStructures;
[DebuggerDisplay("HasValue = {HasValue}, Value = {value}")]
public readonly struct Option<T> : IEquatable<Option<T>>, IEquatable<T> {
    private readonly T? value;
    private readonly bool hasValue;
    public bool IsNone => !hasValue;
    public bool IsSome => hasValue;

    public Option(T? value, bool hasValue) {
        this.value = value;
        this.hasValue = hasValue;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> Some(T value) => new Option<T>(value, true);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> None() => new Option<T>(default, false);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Option<T>(T? value) => value is not null ? Some(value) : None();

    public void Deconstruct(out bool hasValue, out T? value) {
        hasValue = this.hasValue;
        value = this.value;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T GetValueOrThrow() {
        if (IsNone)
            throw new InvalidOperationException("Option has no value.");

        return value!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T GetValueOrDefault(T defaultValue) => hasValue ? value! : defaultValue;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T GetValueOrDefault(Func<T> defaultFunc) => hasValue ? value! : defaultFunc();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T? GetValueOrDefault() => hasValue ? value! : default;

    public R Match<R>(Func<T, R> someFunc, Func<R> noneFunc) {
        return hasValue ? someFunc(value!) : noneFunc();
    }

    public async Task<R> MatchAsync<R>(Func<T, Task<R>> someFunc, Func<Task<R>> noneFunc) {
        return hasValue ? await someFunc(value!) : await noneFunc();
    }

    public Option<U> Map<U>(Func<T, U> mapFunc) {
        return hasValue ? Option<U>.Some(mapFunc(value!)) : Option<U>.None();
    }

    public async Task<Option<U>> MapAsync<U>(Func<T, Task<U>> mapFunc) {
        return hasValue ? Option<U>.Some(await mapFunc(value!)) : Option<U>.None();
    }

    public Option<U> Bind<U>(Func<T, Option<U>> bindFunc) {
        return hasValue ? bindFunc(value!) : Option<U>.None();
    }

    public Task<Option<U>> BindAsync<U>(Func<T, Task<Option<U>>> bindFunc) {
        return hasValue ? bindFunc(value!) : Task.FromResult(Option<U>.None());
    }

    public Option<T> Tap(Action<T> someAction) {
        if (hasValue) {
            someAction(value!);
        }
        return this;
    }

    public async Task<Option<T>> TapAsync(Func<T, Task> someAction) {
        if (hasValue) {
            await someAction(value!);
        }
        return this;
    }

    public override bool Equals(object? obj) {
        return obj is Option<T> other && Equals(other) || obj is T otherValue && Equals(otherValue);
    }

    public bool Equals(Option<T> other) {
        return hasValue == other.hasValue && EqualityComparer<T?>.Default.Equals(value, other.value);
    }

    public bool Equals(T? other) {
        return hasValue && EqualityComparer<T?>.Default.Equals(value, other);
    }

    public override int GetHashCode() {
        return HBHashCode.Combine(hasValue, value);
    }

    public static bool operator ==(Option<T> left, Option<T> right) {
        return left.Equals(right);
    }

    public static bool operator !=(Option<T> left, Option<T> right) {
        return !(left == right);
    }

    public static bool operator ==(T left, Option<T> right) {
        return left != null && right.Equals(left);
    }

    public static bool operator !=(T left, Option<T> right) {
        return !(left == right);
    }
}