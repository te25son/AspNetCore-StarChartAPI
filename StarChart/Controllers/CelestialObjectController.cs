using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

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

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var existing = _context.CelestialObjects.Find(id);

            if (existing == null)
                return NotFound();

            existing.Name = celestialObject.Name;
            existing.OrbitalPeriod = celestialObject.OrbitalPeriod;
            existing.OrbitedObjectId = celestialObject.OrbitedObjectId;

            _context.CelestialObjects.Update(existing);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var existing = _context.CelestialObjects.Find(id);

            if (existing == null)
                return NotFound();
            
            existing.Name = name;
            
            _context.CelestialObjects.Update(existing);
            _context.SaveChanges();
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var celestialObjects = _context.CelestialObjects
                .Where(e => e.Id == id || e.OrbitedObjectId == id);

            if (!celestialObjects.Any())
                return NotFound();
            
            _context.CelestialObjects.RemoveRange(celestialObjects);
            _context.SaveChanges();
            
            return NoContent();
        }
    }
}
