using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using TDMUTestsServer.Domain.Entities.Infrastructure;
using System.Web;
using TDMUTestsServer.Services.Interfaces;
using Microsoft.AspNet.Identity.Owin;
using System.Net.Http;
using TDMUTestsServer.Infrastructure.Business;

namespace TDMUTestsServer.Web.Controllers.ApiControllers.V1
{
    [Authorize]
    [RoutePrefix("api/v1/profile")]
    public class ProfileController : ApiController
    {
        private readonly IUserService _userService;

        public ProfileController(IUserService userService)
        {
            _userService = userService;
        }

        // POST api/Profile/me
        /// <remarks>
        ///     <para>
        ///       Authorize. Return Info for current user
        ///     </para>
        /// </remarks>
        [HttpGet]
        [Route("Me")]
        public async Task<IHttpActionResult> GetCurrentUser()
        {
            var userId = User.Identity.GetUserId();
            var result = await _userService.GetShortUser(userId);
            return Ok(result);
        }


        /// <remarks>
        /// api/v1/athlete/info
        /// Only coaches can use this method
        ///        <para>
        ///        "userId": "string", //Athlete Id. 
        ///        </para>
        /// </remarks>
        /// 

        // [Authorize(Roles = "Coach")]
        [HttpGet]
        [Route("{userId}")]
        public async Task<IHttpActionResult> GetUser(string userId)
        {
            var result = await _userService.GetShortUser(userId);

            return Ok(result);
        }
    }
}