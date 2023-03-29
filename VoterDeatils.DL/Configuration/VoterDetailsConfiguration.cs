using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoterDeatils.DL.Model;

namespace VoterDeatils.DL.Configuration
{
    public class VoterDetailsConfiguration:IEntityTypeConfiguration<VoterDetails>
    {
        public void Configure(EntityTypeBuilder<VoterDetails> builder)
        {
            builder.ToTable("VoterDetails");
            builder.HasKey(t => t.VoterId);
          
        }
    }

  
}
