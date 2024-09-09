using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Results;
public readonly struct ValidationResult {
    public bool IsValid { get; }
    public ImmutableArray<string> Messages { get; }
    public ImmutableArray<Exception> Exceptions { get; }

    public ValidationResult(bool isValid, IEnumerable<string> messages, IEnumerable<Exception> exceptions) {
        IsValid = isValid;
        this.Messages = [.. messages];
        this.Exceptions = [.. exceptions];
    }

    public static ValidationResult Success => new ValidationResult(true, [], []);
    public static ValidationResult Failure(string message) => new ValidationResult(false, [message], []);
    public static ValidationResult Failure(Exception exception) => new ValidationResult(false, [exception.Message], [exception]);
    public static ValidationResult Failure(string message, Exception exception) => new ValidationResult(false, [message], [exception]);

    public static ValidationResult Failure(IEnumerable<string> messages) =>  new ValidationResult(false, messages, []);
    public static ValidationResult Failure(IEnumerable<Exception> exceptions) =>  new ValidationResult(false, exceptions.Select(e => e.Message), exceptions);
    public static ValidationResult Failure(IEnumerable<string> messages, IEnumerable<Exception> exceptions) => new ValidationResult(false, messages, exceptions);

}

