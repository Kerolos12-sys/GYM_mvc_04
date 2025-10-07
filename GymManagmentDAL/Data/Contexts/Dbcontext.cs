using GymManagmentDAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Data.Contexts
{
    public class Dbcontext:DbContext
    {


        public Dbcontext(DbContextOptions<Dbcontext> options) :base(options)
        { 
        
        
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer(
        //            "Server=.;Database=Gym_Database;Trusted_Connection=True;TrustServerCertificate=True;");
        //    }
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public DbSet<Member> Members { get; set; }
        public DbSet<GymUser> GymUsers { get; set; }

        public DbSet<HealthRecord> HealthRecords { get; set; }

        public DbSet<Trainer> Trainers { get; set; }        
        public DbSet<Session> Sessions { get; set; }

        public DbSet<MemberSession> MemberSessions { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Plan> Plans { get; set; }
    }
}
