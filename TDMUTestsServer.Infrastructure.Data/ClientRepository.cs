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
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        public ClientRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }

        public async Task<DatabaseOneResult<Client>> Get(string id)
        {
            return await base.FindOneAsync( _ => _.Id == id );
        }
    }
}
