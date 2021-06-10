using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace RefactorThis
{
    public class BaseController : ControllerBase
    {
        private readonly ILogger _logger;
        public BaseController(ILogger logger)
        {
            _logger = logger;
        }
        protected ActionResult ExceptionHandler(Exception ex)
        {
            // proper logging object with proper fields should be applied, so that, search on logs 
            // will be easier. eg. Insight Query.

            _logger.LogError(ex.Message);

            // TODO: Map the exceptions to the proper http status code, instead of returning 500 for all the exceptions
            return StatusCode(StatusCodes.Status500InternalServerError, new Exception(ex.Message));
        }
    }

}
