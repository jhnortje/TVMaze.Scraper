using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TVMaze.Infrastructure.Data.Entities
{
    public class Show
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int64 ShowId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<CastShow> ShowCasts { get; set; }
    }
}
