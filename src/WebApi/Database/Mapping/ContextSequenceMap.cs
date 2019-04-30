using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Database.Entities;

namespace WebApi.Database.Mapping
{
    public class ContextSequenceMap : IEntityTypeConfiguration<ContextSequenceEty>
    {
        public void Configure(EntityTypeBuilder<ContextSequenceEty> builder)
        {
            builder.HasKey(x => new { x.CompanyId, x.Context });

            builder.Property(x => x.Context).HasMaxLength(20);
            builder.Property(x => x.Value).IsRequired();

            builder.HasOne(x => x.Company).WithMany().HasForeignKey(x => x.CompanyId);
        }
    }
}
