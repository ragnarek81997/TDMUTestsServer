using TDMUTestsServer.Domain.Interfaces.Infrastructure;
using System.Threading.Tasks;
using TDMUTestsServer.Domain.Entities.Infrastructure;
using TDMUTestsServer.Domain.Entities.Models;

namespace TDMUTestsServer.Domain.Interfaces
{
    public interface IDisciplineRepository : IRepository<Discipline>
    {
    }
}
