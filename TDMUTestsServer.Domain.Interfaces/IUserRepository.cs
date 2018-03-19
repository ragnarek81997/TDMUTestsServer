using TDMUTestsServer.Domain.Interfaces.Infrastructure;
using System.Threading.Tasks;
using TDMUTestsServer.Domain.Entities.Infrastructure;
using TDMUTestsServer.Domain.Entities.ViewModels;
using TDMUTestsServer.Domain.Entities.Enums;

namespace TDMUTestsServer.Domain.Interfaces
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        Task<DatabaseOneResult<ApplicationUser>> Get(string id);
    }
}
