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
    internal class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            // Name
            builder.Property(x => x.Name)
                   .HasColumnType("varchar")
                   .HasMaxLength(50);

            // Description
            builder.Property(x => x.Description)
                   .HasColumnType("varchar")
                   .HasMaxLength(100);

            // Price (decimal with precision 10,2)
            builder.Property(x => x.Price)
                   .HasPrecision(10, 2);

            // Table & Constraints
            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint(
                    name: "PlanDurationCheck",
                    sql: "DurationDays BETWEEN 1 AND 365"
                );
            });
        }
    }
}
