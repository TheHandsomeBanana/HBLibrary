using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Code.Interpreter.Lexer;
public delegate bool ReadConditionDelegate();

public class LexContentReader {
    public string Content { get; private set; } = string.Empty;

    public LexContentReader() { }
    public LexContentReader(string content) {
        this.Content = content;
    }

    public int LastIndex { get; private set; } = -1;
    public int LastLine { get; private set; } = 1;
    public int LastLineIndex { get; private set; } = -1;
    public int CurrentIndex { get; private set; } = -1;
    public int CurrentLine { get; private set; } = 1;
    public int CurrentLineIndex { get; private set; } = -1;

    public void Init(string content) {
        this.Content = content;
    }

    public bool CanRead() => CurrentIndex < Content.Length;
    public bool CanRead(int index) => index < Content.Length;
    public bool CanPeek() => CurrentIndex + 1 < Content.Length;
    public bool HasRead() => LastIndex != -1;

    public void SkipSingle() {
        if (Content[CurrentIndex] == CommonCharCollection.LF)
            NewLine();
        else
            CurrentLineIndex++;

        CurrentIndex++;
    }

    public char? ReadSingle() {
        Increment();
        char? value = CanRead() ? GetChar() : null;
        if (HasRead() && CanRead(LastIndex) && Content[LastIndex] == CommonCharCollection.LF)
            NewLine();

        return value;
    }

    public string ReadWhile(ReadConditionDelegate condition, int skipReadsBefore = 0, int skipReadsAfter = 0) {
        LastIndex = CurrentIndex;
        LastLineIndex = CurrentLineIndex;

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < skipReadsBefore && CanRead(); i++) {
            sb.Append(Content[CurrentIndex]);
            SkipSingle();
        }

        int lastIndex = -1;
        while (CanRead() && condition()) {
            sb.Append(Content[CurrentIndex]);

            if (lastIndex != -1 && Content[lastIndex] == CommonCharCollection.LF)
                NewLine();
            else
                CurrentLineIndex++;

            lastIndex = CurrentIndex;
            CurrentIndex++;
        }

        for (int i = 0; i < skipReadsAfter && CanRead(); i++) {
            sb.Append(Content[CurrentIndex]);
            SkipSingle();
        }

        return sb.ToString();
    }

    public char? PeekSingle() {
        return CanPeek() ? Content[CurrentIndex + 1] : null;
    }

    public TextSpan GetSpan() => new TextSpan(
        HasRead() ? LastIndex : 0,
        CurrentIndex - (HasRead() ? LastIndex : CurrentIndex));

    public LineSpan GetLineSpan() {
        int start;
        int length;

        if (!HasRead() || LastLine < CurrentLine) {
            LastLine = CurrentLine;
            start = 0;
            length = CurrentLineIndex;
        }
        else {
            start = LastLineIndex;
            length = CurrentLineIndex - LastLineIndex;
        }

        return new LineSpan(CurrentLine, 0, start, length);
    }

    public char GetChar() => Content[CurrentIndex];
    public char? GetChar(int index) => CanRead(index) ? Content[index] : null;

    public string GetString() {
        StringBuilder sb = new StringBuilder();
        for (int i = LastIndex; i < CurrentIndex; i++)
            sb.Append(Content[i]);

        return sb.ToString();
    }



    private void Increment() {
        LastIndex = CurrentIndex;
        LastLineIndex = CurrentLineIndex;

        CurrentIndex++;
        CurrentLineIndex++;
    }

    private void NewLine() {
        LastLine = CurrentLine;
        CurrentLine++;
        LastLineIndex = -1;
        CurrentLineIndex = 0;
    }


}
