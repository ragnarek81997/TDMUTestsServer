using Newtonsoft.Json.Serialization;
using TDMUTestsServer.Domain.Entities.Struct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace TDMUTestsServer.Web.Results
{
    public class ErrorResult : IHttpActionResult
    {
        ServiceResult _serviceResult;
        HttpRequestMessage _request;
        HttpStatusCode _httpStatusCode = HttpStatusCode.BadRequest;

        public ErrorResult(ServiceResult serviceResult, HttpRequestMessage request)
        {
            _serviceResult = serviceResult;
            _request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var jsonMediaTypeFormatter = new JsonMediaTypeFormatter();
            jsonMediaTypeFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); 

            var response = new HttpResponseMessage()
            {
                Content = new ObjectContent<ServiceResult>(_serviceResult, jsonMediaTypeFormatter),
                RequestMessage = _request,
                StatusCode = _httpStatusCode,
            };

            return Task.FromResult(response);
        }
    }
}