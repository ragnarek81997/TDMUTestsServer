using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDMUTestsServer.Domain.Entities.Enums;

namespace TDMUTestsServer.Domain.Entities.Infrastructure
{
    public class DatabaseResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public DatabaseResultType Type { get; set; }

        public DatabaseResult()
        {
            Success = false;
            Message = string.Empty;
            Exception = null;
            Type = DatabaseResultType.SuccessUpdate;
        }

    }

    public class DatabaseResult<TEntity> : DatabaseResult
    {
        public TEntity Entity { get; set; }
    }

    public class DatabaseOneResult<TEntity> : DatabaseResult where TEntity : IBaseEntity
    {
        public TEntity Entity { get; set; }
    }

    public class DatabaseManyResult<TEntity> : DatabaseResult where TEntity : IBaseEntity
    {
        public List<TEntity> Entities { get; set; }
    }
}
