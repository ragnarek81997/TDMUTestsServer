using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TDMUTestsServer.Domain.Entities.Infrastructure;

namespace TDMUTestsServer.Domain.Entities.Models
{
    [Table("Semesters", Schema = "dbo")]
    public class Semester : BaseEntity
    {
        [Required]
        public int Number { get; set; }

        public virtual ICollection<Discipline> Disciplines { get; set; }
    }
}
