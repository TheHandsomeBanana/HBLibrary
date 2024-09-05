using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Results;
public readonly struct ValidationResult {
    public bool IsValid { get; }
    public string? Message { get; }
    public Exception? Exception { get; }

    public ValidationResult(bool isValid, string? message, Exception? exception) {
        IsValid = isValid;
        Message = message;
        Exception = exception;
    }

    public static ValidationResult Success() => new ValidationResult(true, null, null);
    public static ValidationResult Success(string message) => new ValidationResult(true, message, null);
    public static ValidationResult Failure(string message) => new ValidationResult(false, message, null);
    public static ValidationResult Failure(Exception exception) => new ValidationResult(false, exception.Message, exception);
    public static ValidationResult Failure(string message, Exception exception) => new ValidationResult(false, message, exception);

    public void Deconstruct(out bool isValid, out string? message, out Exception? exception) {
        isValid = IsValid;
        message = Message;
        exception = Exception;
    }
}
