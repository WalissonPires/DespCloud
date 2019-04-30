using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Database.Entities;

namespace WebApi.Database.Mapping
{
    public class AddressMap : IEntityTypeConfiguration<AddressEty>
    {
        public void Configure(EntityTypeBuilder<AddressEty> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.ZipCode).HasMaxLength(8).IsFixedLength();
            builder.Property(x => x.Number).IsRequired().HasMaxLength(30);
            builder.Property(x => x.Street).IsRequired().HasMaxLength(200);
            builder.Property(x => x.DistrictId).IsRequired();
            builder.Property(x => x.DistrictName).IsRequired().HasMaxLength(200);
            builder.Property(x => x.CityId).IsRequired();
            builder.Property(x => x.CityName).IsRequired().HasMaxLength(200);
            builder.Property(x => x.CountyId).IsRequired();
            builder.Property(x => x.CountyName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.CountyInitials).IsRequired().HasMaxLength(2).IsFixedLength();
        }
    }
}
