using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TVMaze.Infrastructure.Data.Entities;

namespace TVMaze.Infrastructure.Data.Mapping
{
    public class CastShowMap
    {
        public CastShowMap(EntityTypeBuilder<CastShow> entityBuilder)
        {
            entityBuilder.HasKey(t => t.CastShowId);
        }
    }
}
