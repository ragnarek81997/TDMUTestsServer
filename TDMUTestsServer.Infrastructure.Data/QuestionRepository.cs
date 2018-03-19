using TDMUTestsServer.Domain.Interfaces.Infrastructure;
using TDMUTestsServer.Domain.Entities.Infrastructure;
using TDMUTestsServer.Domain.Entities.Models;
using TDMUTestsServer.Domain.Interfaces;

namespace TDMUTestsServer.Infrastructure.Data
{
    public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
