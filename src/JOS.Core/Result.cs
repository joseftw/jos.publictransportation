using System.Collections.Generic;
using System.Linq;

namespace JOS.Core
{
    public class Result
    {
        private static readonly Result OkResult = new Result(true);
        private static readonly Result FailResult = new Result(false);
        protected Result(bool success, string message = null) : this(success, message, Enumerable.Empty<Error>()) { }

        protected Result(bool success, string message, IEnumerable<Error> errors)
        {
            Success = success;
            Message = message;
            Errors = errors ?? Enumerable.Empty<Error>();
        }

        public bool Success { get; }
        public string Message { get; }
        public IEnumerable<Error> Errors { get; }
        public Error FirstError => Errors?.FirstOrDefault();

        public static Result Ok() => OkResult;
        public static Result Ok(string message) => new Result(true, message);
        public static Result Fail() => FailResult;
        public static Result Fail(string message) => new Result(false, message);
        public static Result Fail(string message, IEnumerable<Error> errors) => new Result(false, message, errors);
    }

    public class Result<T> : Result
    {
        protected Result(bool success, T data) : this(success, data, null) { }

        protected Result(bool success, T data, string message) : this(success, data, message, Enumerable.Empty<Error>()) { }

        protected Result(bool success, T data, string message, IEnumerable<Error> errors) : base(success, message, errors)
        {
            Data = data;
        }

        public T Data { get; }

        public static Result<T> Ok(T data)
        {
            return new Result<T>(true, data);
        }

        public static Result<T> Ok(T data, string message)
        {
            return new Result<T>(true, data, message);
        }

        public new static Result<T> Fail()
        {
            return new Result<T>(false, default(T));
        }

        public new static Result<T> Fail(string message)
        {
            return new Result<T>(false, default(T), message);
        }

        public new static Result<T> Fail(string message, IEnumerable<Error> errors)
        {
            return new Result<T>(false, default(T), message, errors);
        }
    }

    public class Error
    {
        public Error(string message, ErrorType type) : this(null, message, type)
        {

        }

        public Error(string errorCode, string message, ErrorType errorType)
        {
            ErrorCode = errorCode;
            Message = message;
            Type = errorType;
        }
        public string ErrorCode { get; }
        public string Message { get; }
        public ErrorType Type { get; }
    }

    public enum ErrorType
    {
        Undefined,
        Register,
        NotFound,
        Unathorized
    }
}
