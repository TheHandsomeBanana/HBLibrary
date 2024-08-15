using System.Diagnostics.CodeAnalysis;

namespace HBLibrary.Code.Interpreter;
public readonly struct LineSpan : IEquatable<LineSpan> {
    public int StartLine { get; }
    public int LineLength { get; }
    public int EndLine => StartLine + LineLength;

    public TextSpan Span { get; }

    public LineSpan(int startLine, int lineLength, TextSpan span) {
        StartLine = startLine;
        LineLength = lineLength;
        Span = span;
    }

    public LineSpan(int startLine, int lineLength, int start, int length) {
        StartLine = startLine;
        LineLength = lineLength;
        Span = new TextSpan(start, length);
    }

    public bool Equals(LineSpan other) {
        return StartLine == other.StartLine &&
            LineLength == other.LineLength &&
            Span == other.Span;
    }

    public override bool Equals([NotNullWhen(true)] object? obj) {
        return obj is LineSpan lineSpan && Equals(lineSpan);
    }

    public override int GetHashCode() => HashCode.Combine(StartLine, LineLength, Span);

    public override string? ToString() {
        return $"[{StartLine}..{EndLine}] - {Span}";
    }

    public static bool operator ==(LineSpan left, LineSpan right) {
        return left.Equals(right);
    }

    public static bool operator !=(LineSpan left, LineSpan right) {
        return !(left == right);
    }
}
