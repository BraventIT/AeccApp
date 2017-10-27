using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AeccApi.Models
{
    public class AeccContext : DbContext
    {
        public AeccContext(DbContextOptions<AeccContext> options) : base(options)
        {
        }

        public DbSet<RequestType> RequestTypes { get; set; }
        public DbSet<Coordinator> Coordinators { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<HospitalAssignment> HospitalAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RequestType>().ToTable("RequestType");
            modelBuilder.Entity<Coordinator>().ToTable("Coordinator");
            modelBuilder.Entity<Hospital>().ToTable("Hospital");
            modelBuilder.Entity<HospitalAssignment>().ToTable("HospitalAssignment");

            modelBuilder.Entity<HospitalAssignment>().HasKey(c => new { c.CoordinatorID, c.HospitalID });
        }
    }
}
