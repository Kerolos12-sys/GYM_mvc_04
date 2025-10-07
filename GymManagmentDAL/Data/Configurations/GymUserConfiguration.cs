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
    internal class GymUserConfiguration<T>:IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            // Name
            builder.Property(x => x.Name)
                   .HasColumnType("varchar")
                   .HasMaxLength(50);

            // Email
            builder.Property(x => x.Email)
                   .HasColumnType("varchar")
                   .HasMaxLength(100);

            // Phone
            builder.Property(x => x.Phone)
                   .HasColumnType("varchar")
                   .HasMaxLength(11);

            // Table Constraints
            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint(
                    name: "GymUserValidEmailCheck",
                    sql: "Email LIKE '_%@_%._%'");

                tb.HasCheckConstraint(
                    name: "GymUserValidPhoneCheck",
                    sql: "Phone LIKE '01%' AND Phone NOT LIKE '%[^0-9]%'");
            });

            // Unique Indexes
            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.Phone).IsUnique();

            // Owned Address
            builder.OwnsOne(x => x.Address, addressBuilder =>
            {
                addressBuilder.Property(x => x.Street)
                              .HasColumnName("Street")
                              .HasColumnType("varchar")
                              .HasMaxLength(30);

                addressBuilder.Property(x => x.City)
                              .HasColumnName("City")
                              .HasColumnType("varchar")
                              .HasMaxLength(30);

                addressBuilder.Property(x => x.BuildingNumber)
                              .HasColumnName("BuildingNumber");
            });
        }


    }
}
