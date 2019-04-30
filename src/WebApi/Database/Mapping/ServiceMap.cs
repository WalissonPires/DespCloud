using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Database.Entities;

namespace WebApi.Database.Mapping
{
    public class ServiceMap : IEntityTypeConfiguration<ServiceEty>
    {
        public void Configure(EntityTypeBuilder<ServiceEty> builder)
        {
            builder.HasKey(x => new { x.CompanyId, x.Id });

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Honorary).IsRequired();
            builder.Property(x => x.Rate).IsRequired();
            builder.Property(x => x.PlateCard).IsRequired();
            builder.Property(x => x.Other).IsRequired();

            builder.HasOne(x => x.Company).WithMany().HasForeignKey(x => x.CompanyId);
        }
    }
}
