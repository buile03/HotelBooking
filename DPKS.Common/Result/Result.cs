using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DPKS.Common.Result
{
    public class Result
    {
        public Result(bool isSuccessed)
        {
            IsSuccessed = isSuccessed;
        }

        public Result(bool isSuccessed, string message) : this(isSuccessed)
        {
            IsSuccessed = isSuccessed;
            Message = message;
        }

        public bool IsSuccessed { get; private set; }

        [JsonIgnore]
        public ResultStatus Status { get; private set; }

        public string Message { get; private set; }
        public static Result Success() => new(true) { Status = ResultStatus.Ok };
        public static Result Success(string message) => new(true, message) { Status = ResultStatus.Ok };
        public static Result Error() => new(false) { Status = ResultStatus.Error };
        public static Result Error(string message) => new(false, message) { Status = ResultStatus.Error };
        /// <summary>
        /// Default message: 'Dữ liệu không tồn tại'
        /// </summary>
        public static Result NotFound() => new(false, "Không tìm thấy trang") { Status = ResultStatus.NotFound };
        public static Result NotFound(string valueName) => new(false, $"{valueName} không tìm thấy") { Status = ResultStatus.NotFound };
    }
    public enum ResultStatus
    {
        Ok, 
        Error, 
        NotFound
    }

    public class Result<T>
    {
        public Result(bool isSuccessed)
        {
            IsSuccessed = isSuccessed;
        }
        public Result (bool isSuccessed, string massage, T resultObj) : this(isSuccessed)
        {
            Message = massage;
            ResultObj = resultObj;
        }
        public bool IsSuccessed { get; set; }
        public ResultStatus Status { get;private set; }
        public string Message { get; set; }
        public T ResultObj { get; set; }

        public static implicit operator Result<T>(Result result) => new(result.IsSuccessed, result.Message, default) { Status = result.Status };
        
        public static Result<T> Success(string message, T resultObj) => new(true, message, resultObj) { Status = ResultStatus.Ok };
        public static Result<T> Success(string message) => new(true, message, default) { Status = ResultStatus.Ok };
        public static Result<T> Success(T resultObj) => new(true, "Cập nhật thành công!", resultObj) { Status = ResultStatus.Ok };
        public static Result<T> Error() => new(false) { Status = ResultStatus.Error };
        public static Result<T> Error(string message, T resultObj) => new(false, message, resultObj) { Status = ResultStatus.Error };
        public static Result<T> Error(string message) => new(false, message, default) { Status = ResultStatus.Error };
        public static Result<T> Error(T resultObj) => new(true, "Cập nhật không thành công!", resultObj) { Status = ResultStatus.Error };
        /// <summary>
        /// Default message: 'Dữ liệu không tồn tại'
        /// </summary>
        public static Result<T> NotFound() => new(false, "Không tìm thấy trang", default) { Status = ResultStatus.NotFound };
        public static Result<T> NotFound(string valueName) => new(false, $"{valueName} không tìm thấy", default) { Status = ResultStatus.NotFound };
    }
}
