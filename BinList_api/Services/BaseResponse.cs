using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BinList_api.Services
{
    public class BaseResponse<T>
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public bool isSuccessful { get; set; }
        public T Data { get; set; }
    }
}
