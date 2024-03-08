using UnityEngine;

public enum ErrorCode
{
    // General Errors
    UnknownError = 0, // An unspecified error has occurred.
    OperationFailed = 1, // The operation failed without specific details.

    // Validation Errors
    ValidationError = 100, // General validation failure.
    InvalidInput = 101, // The provided input is invalid.
    MissingRequiredField = 102, // A required input field is missing.
    OutOfRange = 103, // An input value is outside the allowed range.

    // Authentication and Authorization Errors
    Unauthorized = 200, // The user is not authenticated.
    Forbidden = 201, // The user is authenticated but lacks the required permissions.
    SessionExpired = 202, // The user's session has expired, requiring re-authentication.

    // Resource Management Errors
    NotFound = 300, // The specified resource was not found.
    AlreadyExists = 301, // The resource already exists and cannot be created again.
    ResourceLocked = 302, // The resource is locked or in use and cannot be modified.

    // System and Integration Errors
    ExternalServiceError = 400, // There was an error with an external service integration.
    DatabaseError = 401, // A database error occurred, preventing the operation.
    NetworkError = 402, // A network error prevented communication or data transfer.

    // Business Logic Errors
    BusinessRuleViolation = 500, // A business rule was violated, preventing the operation.
    OperationNotSupported = 501, // The requested operation is not supported by the system.
}

public class Result
{
    public bool IsSuccess { get; private set; }
    public bool IsFailure { get => !IsSuccess; }
    public string Error { get; private set; }
    public ErrorCode ErrorCode { get; private set; }

    protected Result(bool isSuccess, string error = null, ErrorCode errorCode = ErrorCode.UnknownError)
    {
        IsSuccess = isSuccess;
        Error = error;
        ErrorCode = errorCode;
    }

    public static Result Success() => new(true);

    public static Result Failure(string error, ErrorCode errorCode = ErrorCode.UnknownError)
    {
        Debug.LogError($"{errorCode}: {error}");
        return new(false, error, errorCode);
    }
}

public class Result<T> : Result
{
    public T Value { get; private set; }

    private Result(bool isSuccess, T value, string error = null, ErrorCode errorCode = ErrorCode.UnknownError) 
        : base(isSuccess, error, errorCode)
    {
        Value = value;
    }

    public static Result<T> Success(T value) => new(true, value);

    public static new Result<T> Failure(string error, ErrorCode errorCode = ErrorCode.UnknownError)
    {
        Debug.LogError($"{errorCode}: {error}");
        return new(false, default, error, errorCode);
    }
}