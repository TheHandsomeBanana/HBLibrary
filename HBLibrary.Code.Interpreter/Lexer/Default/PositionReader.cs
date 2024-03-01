using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Code.Interpreter.Lexer.Default;
public delegate bool ReadConditionDelegate();

public class PositionReader {
    private readonly string content;
    public PositionReader(string content) {
        this.content = content;
    }

    public int LastIndex { get; private set; } = -1;
    public int LastLineIndex { get; private set; } = -1;

    public int CurrentIndex { get; private set; } = -1;
    public int CurrentLine { get; private set; } = 1;
    public int CurrentLineIndex { get; private set; } = -1;

    public char? ReadSingle() {
        Increment();
        char value = GetChar();
        if (HasRead() && content[LastIndex] == CommonCharCollection.LF)
            NewLine();

        return CanRead() ? value : null;
    }

    public string ReadWhile(ReadConditionDelegate condition) {
        LastIndex = CurrentIndex;
        LastLineIndex = CurrentLineIndex;

        StringBuilder sb = new StringBuilder();
        int lastIndex = -1;
        while (condition() && CanRead()) {
            sb.Append(content[CurrentIndex]);

            if (lastIndex != -1 && content[lastIndex] == CommonCharCollection.LF)
                NewLine();
            else
                CurrentLineIndex++;

            lastIndex = CurrentIndex;
            CurrentIndex++;
        }

        return sb.ToString();
    }

    public char? PeekSingle() {
        return CanPeek() ? content[CurrentIndex + 1] : null;
    }

    public TextSpan GetSpan() => new TextSpan(
        HasRead() ? LastIndex : 0,
        CurrentIndex - (HasRead() ? LastIndex : CurrentIndex));

    public LineSpan GetLineSpan() {
        int start;
        int length;
        if (!HasRead()) {
            start = 0;
            length = CurrentLineIndex;
        }
        else if (LastLineIndex < CurrentLineIndex) {
            start = 0;
            length = CurrentLineIndex;
        }
        else {
            start = LastLineIndex;
            length = CurrentLineIndex - LastLineIndex;
        }

        return new LineSpan(CurrentLine, start, length);
    }

    public char GetChar() => content[CurrentIndex];
    public char? GetChar(int index) => CanRead(index) ? content[index] : null;

    public string GetString() {
        StringBuilder sb = new StringBuilder();
        for (int i = LastIndex; i < CurrentIndex; i++)
            sb.Append(content[i]);

        return sb.ToString();
    }

    private void Increment() {
        LastIndex = CurrentIndex;
        LastLineIndex = CurrentLineIndex;

        CurrentIndex++;
        CurrentLineIndex++;
    }

    private void NewLine() {
        CurrentLine++;
        LastLineIndex = -1;
        CurrentLineIndex = 0;
    }

    private bool CanRead() => CurrentIndex < content.Length;
    private bool CanRead(int index) => index < content.Length;
    private bool CanPeek() => CurrentIndex + 1 < content.Length;
    private bool HasRead() => LastIndex != -1;
}
