using TDMUTestsServer.Domain.Interfaces.Infrastructure;
using System.Threading.Tasks;
using TDMUTestsServer.Domain.Entities.Infrastructure;
using TDMUTestsServer.Domain.Entities.ViewModels;
using TDMUTestsServer.Domain.Entities.Enums;
using TDMUTestsServer.Domain.Entities.Models;

namespace TDMUTestsServer.Domain.Interfaces
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<DatabaseOneResult<Client>> Get(string id);
    }
}
