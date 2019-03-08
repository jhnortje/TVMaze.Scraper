using System;
using System.Collections.Generic;
using System.Text;

namespace TVMaze.Infrastructure.Model
{
    public class ShowModel
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public List<CastModel> Cast { get; set; }
    }
}
