using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TVMaze.Infrastructure.Data.Entities
{
    public class CastShow
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 CastShowId { get; set; }
        public Int64 CastId { get; set; }
        public Int64 ShowId { get; set; }
        public virtual Cast Cast { get; set; }
        public virtual Show Show { get; set; }
    }
}
