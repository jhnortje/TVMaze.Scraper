using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TVMaze.Infrastructure.Data.Entities
{
    public class Cast
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int64 CastId { get; set; }
        public string Name { get; set; }
        public string Birthday { get; set; }
        public virtual ICollection<CastShow> CastShows { get; set; }
    }
}
