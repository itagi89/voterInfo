using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VoterDetails.BL.Service;

namespace VoterInfoWeb.Controllers
{
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
        [HttpGet]
        public async Task<IActionResult> Get(string voterId, string name)
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
