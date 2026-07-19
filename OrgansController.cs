using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODMS.Data;
using ODMS.Models;

namespace ODMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrgansController : ControllerBase
    {
        private readonly AppDbContext _db;
        public OrgansController(AppDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _db.Organs
                .Include(o => o.Donor)
                .Include(o => o.Hospital)
                .ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var o = await _db.Organs
                .Include(x => x.Donor)
                .Include(x => x.Hospital)
                .FirstOrDefaultAsync(x => x.OrganID == id);
            return o == null ? NotFound() : Ok(o);
        }

        // GET organs expiring within 24 hours (for dashboard alert)
        [HttpGet("expiring")]
        public async Task<IActionResult> GetExpiring()
        {
            var cutoff = DateTime.Now.AddHours(24);
            var organs = await _db.Organs
                .Include(o => o.Donor)
                .Where(o => o.CurrentStatus == "Available"
                         && o.ViabilityExpiryTime <= cutoff
                         && o.ViabilityExpiryTime >= DateTime.Now)
                .ToListAsync();
            return Ok(organs);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Organ organ)
        {
            _db.Organs.Add(organ);
            await _db.SaveChangesAsync();
            return Ok(organ);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Organ updated)
        {
            var o = await _db.Organs.FindAsync(id);
            if (o == null) return NotFound();
            o.OrganType             = updated.OrganType;
            o.HarvestDateTime       = updated.HarvestDateTime;
            o.ViabilityExpiryTime   = updated.ViabilityExpiryTime;
            o.CurrentStatus         = updated.CurrentStatus;
            o.SpecialNotes          = updated.SpecialNotes;
            o.CompatibleBloodGroups = updated.CompatibleBloodGroups;
            o.HospitalID            = updated.HospitalID;
            await _db.SaveChangesAsync();
            return Ok(o);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var o = await _db.Organs.FindAsync(id);
            if (o == null) return NotFound();
            _db.Organs.Remove(o);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
