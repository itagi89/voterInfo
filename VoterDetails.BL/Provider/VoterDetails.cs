using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VoterDetails.BL.DTO;
using VoterDetails.BL.Service;
using VoterDetailsAPI.Model;
using VoterDetails.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace VoterDetails.BL.Provider
{
    public class VoterDetailList : IVoterDetails
    {
        private readonly IUnitOfWork _unitOfWork;
        public VoterDetailList(IUnitOfWork unitofWork)
        {
            _unitOfWork = unitofWork;
        }
        public async Task<ListResponse<VoterDetailsDTO>> GetVoterDeatilsById(string voterId, string name)
        {
            ListResponse<VoterDetailsDTO> listResponse = default;

            Expression<Func<VoterDeatils.DL.Model.VoterDetails, bool>> expression = e => true;

            if (!string.IsNullOrWhiteSpace(voterId))
            {
                expression = expression.Or(e => e.VoterId.Contains(voterId));
            }
            if (name != null && !string.IsNullOrWhiteSpace(name))
            {
                expression = expression.And(e => e.Name.Contains(name) || e.LastName.Contains(name) || e.NameEnglish.Contains(name) || e.LastNameEnglish.Contains(name));
            }


            var voterListDo = await _unitOfWork.VoterRepository.GetQueryableAsync(filter: expression).AsNoTracking().ToListAsync();

            if (voterListDo != null)
            {
                List<VoterDetailsDTO> voterlist = MapDcToDto(voterListDo);
                listResponse = new ListResponse<VoterDetailsDTO>(voterlist);
            }
            return listResponse;

        }

        private List<VoterDetailsDTO> MapDcToDto(List<VoterDeatils.DL.Model.VoterDetails> voterListDo)
        {
            return voterListDo.Select(x=>( new VoterDetailsDTO()
            {
                Age = x.Age,
                BuildingName = x.BuildingName,
                BuildingNameUnicode = x.BuildingNameUnicode,
                ConstituencyNumber = x.ConstituencyNumber,
                DOB = x.DOB,
                EnglishBoothAddress = x.EnglishBoothAddress,
                Gender = x.Gender,
                LastName = x.LastName,
                LastNameEnglish = x.LastNameEnglish,
                MiddleName = x.MiddleName,
                MNUnicode = x.MNUnicode,
                Name = x.Name,
                NameEnglish = x.NameEnglish,
                PartNumber = x.PartNumber,
                Phone = x.Phone,
                Pincode = x.Pincode,
                Place = x.Place,
                PollingStnAddress = x.PollingStnAddress,
                PollingStnNo = x.PollingStnNo,
                RlnType = x.RlnType,
                SerialNo = x.SerialNo,
                VoterId = x.VoterId

            })).ToList();
        }
    }
}
