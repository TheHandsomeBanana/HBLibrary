using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Results;
public readonly struct SimpleValidationResult {
    public bool IsValid { get; }
    public string? Message { get; }
    public Exception? Exception { get; }

    public SimpleValidationResult(bool isValid, string? message, Exception? exception) {
        IsValid = isValid;
        Message = message;
        Exception = exception;
    }

    public static SimpleValidationResult Success() => new SimpleValidationResult(true, null, null);
    public static SimpleValidationResult Success(string message) => new SimpleValidationResult(true, message, null);
    public static SimpleValidationResult Failure(string message) => new SimpleValidationResult(false, message, null);
    public static SimpleValidationResult Failure(Exception exception) => new SimpleValidationResult(false, exception.Message, exception);
    public static SimpleValidationResult Failure(string message, Exception exception) => new SimpleValidationResult(false, message, exception);



    public void Deconstruct(out bool isValid, out string? message, out Exception? exception) {
        isValid = IsValid;
        message = Message;
        exception = Exception;
    }
}
