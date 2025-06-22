using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational
{
    public class ApiResult
    {
        public bool IsSuc { get; set; }

        public ResultCode code { get; set; }

        public string msg { get; set; }

        public ApiResult(bool IsSuc, ResultCode code, string msg)
        {
            this.IsSuc = IsSuc;
            this.code = code;
            this.msg = msg;
        }

        public static ApiResult Success(ResultCode code)
        {
            return new ApiResult(true, code, "操作成功");
        }
        public static ApiResult Fail(ResultCode code, string reason)
        {
            return new ApiResult(true, code, reason);
        }
    }
    public class ApiResult<T> : ApiResult
    {
        public T data { get; set; }

        public ApiResult(bool IsSuc, ResultCode code, string msg, T data) : base(IsSuc, code, msg)
        {
            this.data = data;
        }

        public static ApiResult<T> Success(ResultCode code, T data)
        {
            return new ApiResult<T>(true, code, "操作成功", data);
        }
        public static ApiResult<T> Fail(ResultCode code, string reason)
        {
            return new ApiResult<T>(true, code, reason, default!);
        }
    }

    public enum ResultCode
    {
        Ok = 200,
        Fail = 500
    }
}
