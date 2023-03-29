using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using VoterDetailsAPI.Model;

namespace VoterDetailsAPI.Controllers
{
    public abstract class BaseController : Controller
    {
        protected IActionResult CustomResponse<T>(ListResponse<T> listResponse) where T : class
        {
            return ListTypeResponse(listResponse, listResponse?.List);
        }

        protected IActionResult ExceptionResponse(Exception ex)
        {
            ObjectResult result = ex switch
            {
                ValidationException => StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, data = ex.Message }),
                _ => StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, data = ex?.InnerException?.Message ?? ex.Message }),
            };
            return result;
        }
        

        protected string BaseUrl => $"{this.HttpContext.Request.Scheme}://{ this.HttpContext.Request.Host.Value}";

       

        #region Private Method
        private IActionResult ListTypeResponse(BaseResponse response, object data)
        {
            if (data == default)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }
            ObjectResult result = response.StatusCode switch
            {
                StatusCodes.Status200OK => Ok(new { status = StatusCodes.Status200OK, data }),
                StatusCodes.Status500InternalServerError => StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, data = response.ErrorList }),
                _ => StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, data }),
            };
            return result;
        }
        #endregion
    }
}
