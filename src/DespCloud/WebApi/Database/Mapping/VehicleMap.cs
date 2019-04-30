using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Database.Entities;

namespace WebApi.Database.Mapping
{
    public class VehicleMap : IEntityTypeConfiguration<VehicleEty>
    {
        public void Configure(EntityTypeBuilder<VehicleEty> builder)
        {
            builder.HasKey(x => new { x.CompanyId, x.Id });

            builder.Property(x => x.Plate).IsRequired().HasMaxLength(7).IsFixedLength();
            builder.Property(x => x.Chassis).HasMaxLength(17).IsFixedLength();
            builder.Property(x => x.Renavam).HasMaxLength(11).IsFixedLength();
            builder.Property(x => x.Model).HasMaxLength(60);
            builder.Property(x => x.Manufacturer).HasMaxLength(60);
            builder.Property(x => x.YearManufacture);
            builder.Property(x => x.ModelYear);
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.Color).HasMaxLength(20);
            builder.Property(x => x.CityName).HasMaxLength(200);
            builder.Property(x => x.CountyName).HasMaxLength(200);
            builder.Property(x => x.CountyInitials).HasMaxLength(2).IsFixedLength();

            builder.HasIndex(x => x.Plate).IsUnique();
            builder.HasIndex(x => x.Chassis).IsUnique();
            builder.HasIndex(x => x.Renavam).IsUnique();

            builder.HasOne(x => x.Company).WithMany().HasForeignKey(x => x.CompanyId);
            builder.HasOne(x => x.Client).WithMany().HasForeignKey(x => new { x.CompanyId, x.ClientId });
            builder.HasOne(x => x.City).WithMany().HasForeignKey(x => x.CityId);
            builder.HasOne(x => x.County).WithMany().HasForeignKey(x => x.CountyId);
        }
    }
}
