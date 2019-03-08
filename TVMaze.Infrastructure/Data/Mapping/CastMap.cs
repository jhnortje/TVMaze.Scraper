using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TVMaze.Infrastructure.Data.Entities;

namespace TVMaze.Infrastructure.Data.Mapping
{
    public class CastMap
    {
        public CastMap(EntityTypeBuilder<Cast> entityBuilder)
        {
            entityBuilder.HasKey(t => t.CastId);
            entityBuilder.Property(t => t.Name).IsRequired();
            entityBuilder.HasMany(t => t.CastShows).WithOne(t => t.Cast).HasForeignKey(e => e.CastId);
        }
    }
}
