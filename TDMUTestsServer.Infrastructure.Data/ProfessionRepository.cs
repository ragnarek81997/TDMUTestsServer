using TDMUTestsServer.Domain.Interfaces.Infrastructure;
using TDMUTestsServer.Domain.Entities.Infrastructure;
using TDMUTestsServer.Domain.Entities.Models;
using TDMUTestsServer.Domain.Interfaces;

namespace TDMUTestsServer.Infrastructure.Data
{
    public class ProfessionRepository : GenericRepository<Profession>, IProfessionRepository
    {
        public ProfessionRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
