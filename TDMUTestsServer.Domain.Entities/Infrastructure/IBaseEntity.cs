using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDMUTestsServer.Domain.Entities.Infrastructure
{
    public interface IBaseEntity
    {
        [Key]
        string Id { get; set; }
    }
}
