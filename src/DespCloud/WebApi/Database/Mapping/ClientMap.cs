using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Database.Entities;

namespace WebApi.Database.Mapping
{
    public class ClientMap : IEntityTypeConfiguration<ClientEty>
    {
        public void Configure(EntityTypeBuilder<ClientEty> builder)
        {
            builder.HasKey(x => new { x.CompanyId, x.Id });

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.CreateAt).IsRequired();
            builder.Property(x => x.CpfCnpj).HasMaxLength(14);
            builder.Property(x => x.Email).HasMaxLength(60);
            builder.Property(x => x.Phone).HasMaxLength(11).IsFixedLength();
            builder.Property(x => x.ContactName).HasMaxLength(60);
            builder.Property(x => x.RgIE).HasMaxLength(20);
            builder.Property(x => x.Org).HasMaxLength(20);

            builder.HasIndex(x => new { x.CompanyId, x.CpfCnpj }).IsUnique();
            builder.HasIndex(x => new { x.CompanyId, x.Email }).IsUnique();
            builder.HasIndex(x => new { x.CompanyId, x.Phone }).IsUnique();
            builder.HasIndex(x => new { x.CompanyId, x.RgIE }).IsUnique();

            builder.HasOne(x => x.Company).WithMany().HasForeignKey(x => x.CompanyId);
            builder.HasOne(x => x.Address).WithMany().HasForeignKey(x => x.AddressId);
        }
    }
}
