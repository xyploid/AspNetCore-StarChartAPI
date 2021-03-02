using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.FirstOrDefault(c => c.Id == id);
            if (celestialObject != null)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == id).ToList();
                return Ok(celestialObject);
            }
            return NotFound();
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {

            var celestialObjects = _context.CelestialObjects.Where(c => c.Name == name).ToList();
            foreach (CelestialObject celestialObject in celestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == celestialObject.Id).ToList();
            }

            if (celestialObjects != null && celestialObjects.Count>0)
            {
                return Ok(celestialObjects);
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();
            foreach (CelestialObject celestialObject in celestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == celestialObject.Id).ToList();
            }

            return Ok(celestialObjects);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestialObject)
        {
            if(celestialObject!=null)
            {
                _context.CelestialObjects.Add(celestialObject);
                _context.SaveChanges();
            }

            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);

        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var cObject = _context.CelestialObjects.FirstOrDefault(c => c.Id == id);

            if (cObject != null)
            {
                cObject.Name = celestialObject.Name;
                cObject.OrbitalPeriod = celestialObject.OrbitalPeriod;
                cObject.OrbitedObjectId = celestialObject.OrbitedObjectId;
                _context.CelestialObjects.Update(cObject);
                _context.SaveChanges();
                return NoContent();
            }

            return NotFound();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var cObject = _context.CelestialObjects.FirstOrDefault(c => c.Id == id);

            if (cObject != null)
            {
                cObject.Name = name;
                _context.CelestialObjects.Update(cObject);
                _context.SaveChanges();
                return NoContent();
            }

            return NotFound();

        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var cObjects = _context.CelestialObjects.Where(c => c.Id == id || c.OrbitedObjectId == id).ToList();

            if (cObjects != null && cObjects.Count >0)
            {
                _context.CelestialObjects.RemoveRange(cObjects);
                _context.SaveChanges();
                return NoContent();
            }

            return NotFound();

        }
    }
}
