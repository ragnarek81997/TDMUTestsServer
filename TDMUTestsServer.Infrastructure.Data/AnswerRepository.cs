using TDMUTestsServer.Domain.Interfaces.Infrastructure;
using TDMUTestsServer.Domain.Entities.Infrastructure;
using TDMUTestsServer.Domain.Entities.Models;
using TDMUTestsServer.Domain.Interfaces;

namespace TDMUTestsServer.Infrastructure.Data
{
    public class AnswerRepository : GenericRepository<Answer>, IAnswerRepository
    {
        public AnswerRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
