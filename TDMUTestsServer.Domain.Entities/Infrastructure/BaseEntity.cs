using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDMUTestsServer.Domain.Entities.Infrastructure
{
    public abstract class BaseEntity : IBaseEntity
    {
        [Key]
        public string Id { get; set; }
    }
}
