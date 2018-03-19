using TDMUTestsServer.Domain.Interfaces;
using TDMUTestsServer.Domain.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDMUTestsServer.Domain.Entities.Infrastructure;
using TDMUTestsServer.Domain.Entities.ViewModels;
using Microsoft.AspNet.Identity;
using TDMUTestsServer.Domain.Entities.Enums;
using TDMUTestsServer.Domain.Entities.Models;

namespace TDMUTestsServer.Infrastructure.Data
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }

        public async Task<DatabaseResult> Add(RefreshToken model)
        {
            return await base.InsertOneAsync(model);
        }

        public async Task<DatabaseResult> Delete(string id)
        {
            return await base.DeleteOneAsync(_ => _.Id == id );
        }

        public async Task<DatabaseOneResult<RefreshToken>> Get(string id)
        {
            return await base.FindOneAsync(_ => _.Id == id);
        }

        public async Task<DatabaseManyResult<RefreshToken>> GetAll()
        {
            return await base.GetAllWithOrderAscAsync(isAll: true);
        }
    }
}
