using System;
using System.Collections.Generic;
using System.Text;

namespace TVMaze.ScraperService.RequestHandlers
{
    public interface IRequestHandler
    {
        string GetSourceTvMazeContent(string url);
    }
}
