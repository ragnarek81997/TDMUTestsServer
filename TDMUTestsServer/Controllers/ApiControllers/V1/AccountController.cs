using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using TDMUTestsServer.Domain.Entities.Enums;
using TDMUTestsServer.Domain.Entities.Infrastructure;
using TDMUTestsServer.Domain.Entities.Struct;
using TDMUTestsServer.Domain.Entities.ViewModels;
using TDMUTestsServer.Services.Interfaces;
using TDMUTestsServer.Web.Extensions;
using TDMUTestsServer.Web.Providers;
using TDMUTestsServer.Web.Results;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TDMUTestsServer.Infrastructure.Business;

namespace TDMUTestsServer.Web.Controllers.ApiControllers.V1
{
    [Authorize]
    [RoutePrefix("api/v1/account")]
    public class AccountController : CustomApiController
    {
        private ApplicationUserManager _userManager;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public AccountController(ApplicationUserManager userManager, ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel coach)
        {
            var result = new ServiceResult();

            if (ModelState.IsValid)
            {
                result = await _accountService.Register(coach);

                //var recaptchaResult = ValidRecaptcha(coach.Captcha);

                //if (recaptchaResult)
                //{
                //    result = await _accountService.Register(coach);
                //}
                //else
                //{
                //    result.Error.Code = ErrorStatusCode.Recaptcha;
                //    result.Error.Description = "Recaptcha doesn't valid. Try again.";
                //    result.Success = false;
                //}
            }
            else
            {
                result = ModelState.ToServiceResult();
            }

            return ServiceResult(result);
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // POST api/Account/ChangePassword
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            var result = new ServiceResult();

            if (ModelState.IsValid)
            {
                var changePasswordResult = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                result = GetServiceResult(changePasswordResult);
            }
            else
            {
                result = ModelState.ToServiceResult();
            }

            return ServiceResult(result);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private bool ValidRecaptcha(string googleResponse)
        {
            string secretKey = ConfigurationManager.AppSettings["ReCaptchaKey"];//приватний ключ
            var client = new WebClient();
            var resultValid = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, googleResponse));
            var obj = JObject.Parse(resultValid);
            return (bool)obj.SelectToken("success");
        }

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        #endregion
    }
}
