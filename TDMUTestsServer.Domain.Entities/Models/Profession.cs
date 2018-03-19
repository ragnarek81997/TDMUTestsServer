using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TDMUTestsServer.Domain.Entities.Infrastructure;

namespace TDMUTestsServer.Domain.Entities.Models
{
    [Table("Professions", Schema = "dbo")]
    public class Profession : BaseEntity
    {
        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string Name { get; set; }

        public virtual ICollection<Semester> Semesters { get; set; }
    }
}
