using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoterDetails.BL.DTO
{
    public class VoterDetailsDTO
    {
        public int? ConstituencyNumber { get; set; }

        public int? PartNumber { get; set; }

        public int? SerialNo { get; set; }
     
        public string VoterId { get; set; }

        public string NameEnglish { get; set; }

        public string LastNameEnglish { get; set; }

        public string MiddleName { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string MNUnicode { get; set; }

        public string RlnType { get; set; }

        public int? Age { get; set; }

        public string Gender { get; set; }

        public string DOB { get; set; }

        public string Phone { get; set; }

        public string BuildingName { get; set; }

        public string BuildingNameUnicode { get; set; }

        public string EnglishBoothAddress { get; set; }

        public string PollingStnAddress { get; set; }

        public string PollingStnNo { get; set; }

        public string Place { get; set; }

        public string Pincode { get; set; }

    }
}
