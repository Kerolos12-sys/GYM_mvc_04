using GymManagmentDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Data.Configurations
{
    internal class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint(
                    name: "SessionCapacityCheck",
                    sql: "Capacity BETWEEN 1 AND 25"
                );

                tb.HasCheckConstraint(
                    name: "SessionEndDateCheck",
                    sql: "EndDate > StartDate"
                );
            });



            builder.HasOne(x => x.SessionCategory)
                .WithMany(x => x.Sessions)
                .HasForeignKey(x => x.CategoryId);



            builder.HasOne(x=>x.Trainer)
                   .WithMany(x=>x.TrainerSessions)
                   .HasForeignKey(x=>x.TrainerId);
        }
    }
}
