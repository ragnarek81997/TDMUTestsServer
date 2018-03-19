using Microsoft.AspNet.Identity;
using TDMUTestsServer.Domain.Entities.Enums;
using TDMUTestsServer.Domain.Entities.Struct;
using TDMUTestsServer.Web.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace TDMUTestsServer.Web.Controllers.ApiControllers
{
    public class CustomApiController : ApiController
    {
        public IHttpActionResult ServiceResult<T>(ServiceResult<T> source) where T : class
        {
            if (source.Success)
            {
                return Ok(source.Result);
            }
            else
            {
                return new ErrorResult(source, Request);
            }
        }

        public IHttpActionResult ServiceResult(ServiceResult source)
        {
            if (source.Success)
            {
                return Ok();
            }
            else
            {
                return new ErrorResult(source, Request);
            }
        }

        protected ServiceResult GetServiceResult(IdentityResult identityResult)
        {
            var result = new ServiceResult();

            if (result == null)
            {
                result.Error.Code = ErrorStatusCode.InternalServerError;
                result.Error.Description = result.Error.Code.ToString();
            }
            else
            {
                result.Success = identityResult.Succeeded;
                if (!identityResult.Succeeded)
                {
                    result.Error.Description = identityResult?.Errors?.FirstOrDefault();
                    result.Error.Code = ErrorStatusCode.BudRequest;
                }
            }

            return result;
        }
    }
}
