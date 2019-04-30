using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Database.Entities;

namespace WebApi.Database.Mapping
{
    public class UserMap : IEntityTypeConfiguration<UserEty>
    {
        public void Configure(EntityTypeBuilder<UserEty> builder)
        {
            builder.HasKey(x => new { x.CompanyId, x.Id });

            builder.Property(x => x.Name).IsRequired().HasMaxLength(60);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Password).IsRequired().HasMaxLength(30);

            builder.HasIndex(x => new { x.CompanyId, x.Email }).IsUnique();
            builder.HasIndex(x => new { x.Email, x.Password });

            builder.HasOne(x => x.Company).WithMany().HasForeignKey(x => x.CompanyId);
        }
    }
}
