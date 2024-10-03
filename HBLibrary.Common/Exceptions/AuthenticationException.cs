using System.Diagnostics.CodeAnalysis;

namespace HBLibrary.Common.Exceptions;
public class AuthenticationException : Exception {
    public AuthenticationException(string? message) : base(message) {
    }

    public AuthenticationException(string? message, Exception? innerException) : base(message, innerException) {
    }

    [DoesNotReturn]
    public static void ThrowInvalidCredentials() => throw new AuthenticationException("Credentials are invalid.");

    [DoesNotReturn]
    public static void ThrowAuthenticationFailed(Exception exception) => throw new AuthenticationException("Authentication failed.", exception);

    [DoesNotReturn]
    public static void ThrowUserNotFound() => throw new AuthenticationException("User does not exist.");

    [DoesNotReturn]
    public static void ThrowUserAlreadyExists() => throw new AuthenticationException("User already exists.");

    public static AuthenticationException AuthenticationFailed(Exception exception) => new AuthenticationException("Authentication failed.", exception);

    public static AuthenticationException UserAlreadyExists() => new AuthenticationException("User already exists.");

}
