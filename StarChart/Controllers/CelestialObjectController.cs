using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [ApiController]
    [Route("")]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}"), ActionName("GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects
                .Where(o => o.Id == id)
                .FirstOrDefault();

            if (celestialObject == null)
                return NotFound();

            var satellites = _context.CelestialObjects
                .Where(o => o.OrbitedObjectId == id)
                .ToList();

            celestialObject.Satellites = satellites;
            _context.SaveChanges();

            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObject = _context.CelestialObjects
                .Where(o => o.Name == name)
                .FirstOrDefault();

            if (celestialObject == null)
                return NotFound();

            var satellites = _context.CelestialObjects
                .Where(o => o.OrbitedObjectId == o.Id)
                .ToList();

            celestialObject.Satellites = satellites;
            _context.SaveChanges();

            return Ok(celestialObject);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();

            foreach (var celestialObject in celestialObjects)
            {
                var satellites = _context.CelestialObjects
                    .Where(o => o.OrbitedObjectId == o.Id)
                    .ToList();

                celestialObject.Satellites = satellites;
                _context.SaveChanges();
            }

            return Ok(celestialObjects);
        }
    }
}
