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
    [RoutePrefix("api/v1/profession")]
    public class ProfessionController : CustomApiController
    {
        private readonly IProfessionService _professionService;

        public ProfessionController(IProfessionService professionService)
        {
            _professionService = professionService;
        }
    }
}
