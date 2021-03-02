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
    }
}
