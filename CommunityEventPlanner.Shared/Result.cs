using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityEventPlanner.Shared
{
    public class Result<T>
    {
        public Result(T value)
        {
            Success = true;
            HttpStatusCode = 200;
            Value = value;
        }

        public bool Success { get; set; }
        public int HttpStatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Value { get; set; }
    }
}
