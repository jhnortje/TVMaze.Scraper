using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TVMaze.Infrastructure.Data.Entities;

namespace TVMaze.Infrastructure.Data.Mapping
{
    public class ShowMap
    {
        public ShowMap(EntityTypeBuilder<Show> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ShowId);
            entityBuilder.Property(t => t.Name).IsRequired();
            entityBuilder.HasMany(t => t.ShowCasts).WithOne(t => t.Show).HasForeignKey(e => e.ShowId);
        }
    }
}
