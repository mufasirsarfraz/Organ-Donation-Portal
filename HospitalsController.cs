using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODMS.Data;
using ODMS.Models;

namespace ODMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HospitalsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public HospitalsController(AppDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _db.Hospitals.ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var h = await _db.Hospitals.FindAsync(id);
            return h == null ? NotFound() : Ok(h);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Hospital hospital)
        {
            _db.Hospitals.Add(hospital);
            await _db.SaveChangesAsync();
            return Ok(hospital);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Hospital updated)
        {
            var h = await _db.Hospitals.FindAsync(id);
            if (h == null) return NotFound();
            h.Name             = updated.Name;
            h.Location         = updated.Location;
            h.HospitalCategory = updated.HospitalCategory;
            h.ICUCapacity      = updated.ICUCapacity;
            await _db.SaveChangesAsync();
            return Ok(h);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var h = await _db.Hospitals.FindAsync(id);
            if (h == null) return NotFound();
            _db.Hospitals.Remove(h);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
