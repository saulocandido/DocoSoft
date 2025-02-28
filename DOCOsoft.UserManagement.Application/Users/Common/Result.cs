using DOCOsoft.UserManagement.Application.Interfaces;
using System.Collections.Generic;

namespace DOCOsoft.UserManagement.Application.Users.Common
{
    /// <summary>
    /// Class to represent a handled response.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T> where T : IDto
    {
        public bool IsSuccess { get; }
        public string Message { get; }
        public T? Data { get; }
        public List<string> Errors { get; } = new();

        public Result(T data) : this(true, "Success", data, null) { }

        private Result(bool isSuccess, string message, T? data = default, List<string>? errors = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
            if (errors != null)
                Errors.AddRange(errors);
        }

        /// <summary>
        /// ✅ Creates a successful result with optional message.
        /// </summary>
        public static Result<T> Success(T data, string message = "Success") =>
            new Result<T>(true, message, data);

        /// <summary>
        /// ✅ Creates a failed result with multiple errors.
        /// </summary>
        public static Result<T> Failure(List<string> errors, string message = "An error occurred") =>
            new Result<T>(false, message, default, errors);

        /// <summary>
        /// ✅ Creates a failed result with a single error.
        /// </summary>
        public static Result<T> Failure(string error, string message = "An error occurred") =>
            new Result<T>(false, message, default, new List<string> { error });
    }
}
