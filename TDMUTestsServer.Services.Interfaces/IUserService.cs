using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDMUTestsServer.Domain.Entities.Infrastructure;
using TDMUTestsServer.Domain.Entities.Struct;
using TDMUTestsServer.Domain.Entities.ViewModels;

namespace TDMUTestsServer.Services.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult<ApplicationUser>> GetApplicationUser(string userId);

        Task<ServiceResult<ShortUser>> GetShortUser(string userId);
    }
}
