using Newtonsoft.Json;
using TDMUTestsServer.Domain.Entities.ResultModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDMUTestsServer.Domain.Entities.Struct
{
    public class ServiceResult
    {
        [JsonIgnore]
        public bool Success { get; set; }
        public Error Error { get; set; } = new Error();
    }
    public class ServiceResult<T> : ServiceResult where T : class
    {
        public ServiceResult()
        {
        }

        public ServiceResult(ServiceResult serviceResult)
        {
            Success = serviceResult.Success;
            Error = serviceResult.Error;
        }

        public T Result { get; set; }
    }
}
