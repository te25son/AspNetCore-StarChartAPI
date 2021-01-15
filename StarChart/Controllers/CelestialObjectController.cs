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
            var celestialObject = _context.CelestialObjects.Find(id);

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
            var celestialObjects = _context.CelestialObjects
                .Where(o => o.Name == name);

            if (!celestialObjects.Any())
                return NotFound();

            foreach (var celestialObject in celestialObjects)
            {
                var satellites = _context.CelestialObjects
                    .Where(o => o.OrbitedObjectId == celestialObject.Id)
                    .ToList();

                celestialObject.Satellites = satellites;
                _context.SaveChanges();
            }

            return Ok(celestialObjects.ToList());
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();

            foreach (var celestialObject in celestialObjects)
            {
                var satellites = _context.CelestialObjects
                    .Where(o => o.OrbitedObjectId == celestialObject.Id)
                    .ToList();

                celestialObject.Satellites = satellites;
                _context.SaveChanges();
            }

            return Ok(celestialObjects);
        }
    }
}
