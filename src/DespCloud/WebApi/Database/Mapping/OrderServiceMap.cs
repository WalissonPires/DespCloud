using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Database.Entities;

namespace WebApi.Database.Mapping
{
    public class OrderServiceMap : IEntityTypeConfiguration<OrderServiceEty>
    {
        public void Configure(EntityTypeBuilder<OrderServiceEty> builder)
        {
            builder.HasKey(x => new { x.CompanyId, x.Id });

            builder.Property(x => x.CreateAt).IsRequired();
            builder.Property(x => x.ClosedAt);
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.Total).IsRequired();

            builder.HasOne(x => x.Company).WithMany().HasForeignKey(x => x.CompanyId);
            builder.HasOne(x => x.Client).WithMany().HasForeignKey(x => new { x.CompanyId, x.ClientId }).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
