using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDMUTestsServer.Domain.Entities.Enums;

namespace TDMUTestsServer.Domain.Entities.ResultModels
{
    public class Error
    {
        public ErrorStatusCode Code { get; set; }
        public string Description { get; set; }
    }
}
