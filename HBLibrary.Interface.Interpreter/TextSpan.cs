﻿using HBLibrary.Core;

namespace HBLibrary.Interface.Interpreter;

public readonly struct TextSpan : IEquatable<TextSpan>, IComparable<TextSpan> {
    public TextSpan(int start, int length) {
        if (start < 0)
            throw new ArgumentOutOfRangeException(nameof(start));

        if (start > start + length)
            throw new ArgumentOutOfRangeException(nameof(length));

        Start = start;
        Length = length;
    }

    public int Start { get; }
    public int End => Start + Length;
    public int Length { get; }
    public bool IsEmpty => Length == 0;
    public bool Contains(int position) {
        return unchecked((uint)(position - Start) < (uint)Length);
    }
    public bool Contains(TextSpan span) {
        return span.Start >= Start && span.End <= End;
    }

    public bool OverlapsWith(TextSpan span) {
        int overlapStart = Math.Max(Start, span.Start);
        int overlapEnd = Math.Min(End, span.End);

        return overlapStart < overlapEnd;
    }

    public bool IntersectsWith(TextSpan span) {
        return span.Start <= End && span.End >= Start;
    }

    public bool IntersectsWith(int position) {
        return unchecked((uint)(position - Start) <= (uint)Length);
    }

    public static bool operator ==(TextSpan left, TextSpan right) {
        return left.Equals(right);
    }

    public static bool operator !=(TextSpan left, TextSpan right) {
        return !left.Equals(right);
    }

    public bool Equals(TextSpan other) {
        return Start == other.Start && Length == other.Length;
    }

    public override bool Equals(object? obj)
        => obj is TextSpan span && Equals(span);

    public override int GetHashCode() {
        return HBHashCode.Combine(Start, Length);
    }

    public override string ToString() {
        return $"[{Start}..{End}]";
    }

    public int CompareTo(TextSpan other) {
        var diff = Start - other.Start;
        if (diff != 0) {
            return diff;
        }

        return Length - other.Length;
    }
}

