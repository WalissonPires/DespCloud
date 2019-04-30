using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Database.Entities;

namespace WebApi.Database.Mapping
{
    public class CompanyMap : IEntityTypeConfiguration<CompanyEty>
    {
        public void Configure(EntityTypeBuilder<CompanyEty> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.CpfCnpj).IsRequired().HasMaxLength(14);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(60);
            builder.Property(x => x.Phone).IsRequired().HasMaxLength(11).IsFixedLength();
            builder.Property(x => x.LogoPath).HasMaxLength(300);

            builder.HasIndex(x => x.CpfCnpj).IsUnique();
            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.Phone).IsUnique();

            builder.HasOne(x => x.Address).WithMany().HasForeignKey(x => x.AddressId);
        }
    }
}
