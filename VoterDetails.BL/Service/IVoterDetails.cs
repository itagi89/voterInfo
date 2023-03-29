using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoterDetails.BL.DTO;
using VoterDetailsAPI.Model;

namespace VoterDetails.BL.Service
{
    public interface IVoterDetails
    {
        Task<ListResponse<VoterDetailsDTO>> GetVoterDeatilsById(string voterId,string name);
    }
}
