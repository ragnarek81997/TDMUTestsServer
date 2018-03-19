using TDMUTestsServer.Domain.Interfaces.Infrastructure;
using TDMUTestsServer.Domain.Entities.Infrastructure;
using TDMUTestsServer.Domain.Entities.Models;
using TDMUTestsServer.Domain.Interfaces;

namespace TDMUTestsServer.Infrastructure.Data
{
    public class SemesterRepository : GenericRepository<Semester>, ISemesterRepository
    {
        public SemesterRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
