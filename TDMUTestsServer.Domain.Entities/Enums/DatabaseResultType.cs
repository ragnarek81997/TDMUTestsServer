using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDMUTestsServer.Domain.Entities.Enums
{
    public enum DatabaseResultType
    {
        SuccessUpdate = 0,
        NotMatch = 1,
        NotModified = 2, // object for update is identical to object in data base
        ErrorUpdate = 3
    }
}
