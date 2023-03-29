using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoterDeatils.DL.Configuration;
using VoterDeatils.DL.Model;

namespace VoterDeatils.DL
{
    public class VoterDetailsDBContext : DbContext
    {

        public VoterDetailsDBContext(DbContextOptions<VoterDetailsDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          

            //Master Entries
            modelBuilder.ApplyConfiguration(new VoterDetailsConfiguration());
        }
        public DbSet<VoterDetails> VoterDeatils { get; set; }
    }
}
