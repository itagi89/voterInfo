using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoterDetails.BL.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VoterDetailsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoterDetailsController : BaseController
    {

        private readonly ILogger<VoterDetailsController> _logger;
        private readonly IVoterDetails _voterDetails;

        public VoterDetailsController(ILogger<VoterDetailsController> logger, IVoterDetails voterDetails)
        {
            _logger = logger;
            _voterDetails = voterDetails;
        }

        // GET api/<VoterDetailsController>/5
        [HttpGet("{voterId}")]
        public async Task<IActionResult> Get(string  voterId,string name)
        {
            try
            {
                var listResponse = await _voterDetails.GetVoterDeatilsById(voterId, name);
                return CustomResponse(listResponse);
            }
            catch (Exception ex)
            {
                return ExceptionResponse(ex);
            }
        }

      

       
    }
}
