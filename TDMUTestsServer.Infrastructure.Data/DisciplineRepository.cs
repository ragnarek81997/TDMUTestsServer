using TDMUTestsServer.Domain.Interfaces.Infrastructure;
using TDMUTestsServer.Domain.Entities.Infrastructure;
using TDMUTestsServer.Domain.Entities.Models;
using TDMUTestsServer.Domain.Interfaces;

namespace TDMUTestsServer.Infrastructure.Data
{
    public class DisciplineRepository : GenericRepository<Discipline>, IDisciplineRepository
    {
        public DisciplineRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
