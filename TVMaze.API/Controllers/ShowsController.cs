using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TVMaze.Infrastructure.Data.Repository;
using TVMaze.Infrastructure.Model;

namespace TVMaze.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowsController : Controller
    {
        private readonly IRepository<ShowModel> _showCastDataRepository;
        private const int _pageSize = 250;

        public ShowsController(IRepository<ShowModel> showCastDataRepository)
        {
            _showCastDataRepository = showCastDataRepository;
        }

        // http://localhost:59661/api/cast
        // OR http://localhost:59661/api/cast?page=2
        [HttpGet]
        public IActionResult Get(int? page = null)
        {
            // Default to first page if parameter is not supplied
            long rangeToId = ((page ?? 0) + 1) * _pageSize;
            IEnumerable<ShowModel> showModel = _showCastDataRepository.GetRange((rangeToId - _pageSize) + 1, rangeToId);

            return Ok(showModel);
        }
    }
}