using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODMS.Data;
using ODMS.Models;

namespace ODMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganPreservationsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public OrganPreservationsController(AppDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _db.OrganPreservations.Include(op => op.Organ).ToListAsync());

        // GET by composite key: api/organpreservations/P001/ORG001
        [HttpGet("{preservationNumber}/{organId}")]
        public async Task<IActionResult> GetById(string preservationNumber, string organId)
        {
            var op = await _db.OrganPreservations
                .Include(x => x.Organ)
                .FirstOrDefaultAsync(x => x.PreservationNumber == preservationNumber
                                       && x.OrganID == organId);
            return op == null ? NotFound() : Ok(op);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrganPreservation preservation)
        {
            _db.OrganPreservations.Add(preservation);
            await _db.SaveChangesAsync();
            return Ok(preservation);
        }

        [HttpPut("{preservationNumber}/{organId}")]
        public async Task<IActionResult> Update(string preservationNumber, string organId, OrganPreservation updated)
        {
            var op = await _db.OrganPreservations
                .FirstOrDefaultAsync(x => x.PreservationNumber == preservationNumber
                                       && x.OrganID == organId);
            if (op == null) return NotFound();
            op.TechnologyName      = updated.TechnologyName;
            op.StorageTemperature  = updated.StorageTemperature;
            op.MachinePerfusionUsed= updated.MachinePerfusionUsed;
            op.MaxViabilityHours   = updated.MaxViabilityHours;
            await _db.SaveChangesAsync();
            return Ok(op);
        }

        [HttpDelete("{preservationNumber}/{organId}")]
        public async Task<IActionResult> Delete(string preservationNumber, string organId)
        {
            var op = await _db.OrganPreservations
                .FirstOrDefaultAsync(x => x.PreservationNumber == preservationNumber
                                       && x.OrganID == organId);
            if (op == null) return NotFound();
            _db.OrganPreservations.Remove(op);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
