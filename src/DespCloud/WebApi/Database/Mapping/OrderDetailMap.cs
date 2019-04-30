using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Database.Entities;

namespace WebApi.Database.Mapping
{
    public class OrderDetailMap : IEntityTypeConfiguration<OrderDetailEty>
    {
        public void Configure(EntityTypeBuilder<OrderDetailEty> builder)
        {
            builder.HasKey(x => new { x.CompanyId, x.Id });

            builder.Property(x => x.Honorary).IsRequired();
            builder.Property(x => x.Rate).IsRequired();
            builder.Property(x => x.PlateCard).IsRequired();
            builder.Property(x => x.Other).IsRequired();
            builder.Property(x => x.Total).IsRequired();

            builder.HasOne(x => x.Order).WithMany(y => y.Items).HasForeignKey(x => new { x.CompanyId, x.OrderId });
            builder.HasOne(x => x.Vehicle).WithMany().HasForeignKey(x => new { x.CompanyId, x.VehicleId }).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Service).WithMany().HasForeignKey(x => new { x.CompanyId, x.ServiceId }).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
