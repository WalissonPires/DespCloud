using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Database.Entities;

namespace WebApi.Database.Mapping
{
    public class AddressCountyMap : IEntityTypeConfiguration<AddressCountyEty>
    {
        public void Configure(EntityTypeBuilder<AddressCountyEty> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Initials).HasMaxLength(2).IsFixedLength().IsRequired();
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        }
    }
}
