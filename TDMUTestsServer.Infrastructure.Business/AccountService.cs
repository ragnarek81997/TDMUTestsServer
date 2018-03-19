using TDMUTestsServer.Domain.Interfaces;
using TDMUTestsServer.Services.Interfaces;
using System.Threading.Tasks;
using TDMUTestsServer.Domain.Entities.ViewModels;
using TDMUTestsServer.Domain.Entities.Struct;
using Microsoft.AspNet.Identity;
using TDMUTestsServer.Domain.Entities.Infrastructure;
using System;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using TDMUTestsServer.Domain.Entities.Enums;
using System.Web.Http;
using TDMUTestsServer.Infrastructure.Data.Utility.AzureBlob;
using TDMUTestsServer.Domain.Entities.Dictionaries;
using System.Linq;
using TDMUTestsServer.Infrastructure.Data;

namespace TDMUTestsServer.Infrastructure.Business
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        private readonly IUserRepository _userRepository;

        public AccountService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;

            _userRepository = new UserRepository(_applicationDbContext);
        }

        #region Initialization
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        #endregion

        public async Task<ServiceResult> Register(RegisterBindingModel model)
        {
            var serviceResult = new ServiceResult();

            var user = new ApplicationUser()
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await UserManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                UserManager.AddToRole(user.Id, UserRoles.User.ToString());
                serviceResult.Success = true;
            }
            else
            {
                serviceResult.Error.Code = ErrorStatusCode.InvalidSignUp;
                serviceResult.Error.Description = result?.Errors?.FirstOrDefault();
            }

            return serviceResult;
        }
    }
}
