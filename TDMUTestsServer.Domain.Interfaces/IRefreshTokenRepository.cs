using TDMUTestsServer.Domain.Interfaces.Infrastructure;
using System.Threading.Tasks;
using TDMUTestsServer.Domain.Entities.Infrastructure;
using TDMUTestsServer.Domain.Entities.ViewModels;
using TDMUTestsServer.Domain.Entities.Enums;
using TDMUTestsServer.Domain.Entities.Models;

namespace TDMUTestsServer.Domain.Interfaces
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task<DatabaseOneResult<RefreshToken>> Get(string id);
        Task<DatabaseResult> Add(RefreshToken model);
        Task<DatabaseResult> Delete(string id);
        Task<DatabaseManyResult<RefreshToken>> GetAll();
    }
}
