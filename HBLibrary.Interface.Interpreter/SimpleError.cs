﻿using HBLibrary.Interface.Interpreter.Syntax;
using System.Text;

namespace HBLibrary.Interface.Interpreter;
public struct SimpleError {
    public TextSpan Span { get; }
    public LineSpan? LineSpan { get; }
    public string? Affected { get; private set; }
    public string Message { get; }

    public SimpleError(TextSpan span, LineSpan lineSpan, string message, string? affected = null) {
        Span = span;
        LineSpan = lineSpan;
        Message = message;
        Affected = affected;
    }

    public void SetAffected(string content) {
        StringBuilder sb = new StringBuilder();
        for (int i = Span.Start; i <= Span.End; i++) {
            sb.Append(content[i]);
        }

        Affected = sb.ToString();
    }

    public static string GetAffectedString(ISyntaxNode node, string content) {
        StringBuilder sb = new StringBuilder();
        for (int i = node.Span.Start; i <= node.Span.End; i++) {
            sb.Append(content[i]);
        }

        return sb.ToString();
    }

    public override string ToString() {
        return $"{Span} {LineSpan}: {Affected}. Reason: {Message}.";
    }
}