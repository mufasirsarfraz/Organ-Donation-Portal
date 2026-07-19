using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODMS.Data;
using ODMS.Models;

namespace ODMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsentRecordsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ConsentRecordsController(AppDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _db.ConsentRecords.Include(c => c.Donor).ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var c = await _db.ConsentRecords.Include(x => x.Donor)
                        .FirstOrDefaultAsync(x => x.ConsentID == id);
            return c == null ? NotFound() : Ok(c);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ConsentRecord consent)
        {
            _db.ConsentRecords.Add(consent);
            await _db.SaveChangesAsync();
            return Ok(consent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, ConsentRecord updated)
        {
            var c = await _db.ConsentRecords.FindAsync(id);
            if (c == null) return NotFound();
            c.ConsentDate  = updated.ConsentDate;
            c.ConsentType  = updated.ConsentType;
            c.WitnessName  = updated.WitnessName;
            c.CNICVerified = updated.CNICVerified;
            await _db.SaveChangesAsync();
            return Ok(c);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var c = await _db.ConsentRecords.FindAsync(id);
            if (c == null) return NotFound();
            _db.ConsentRecords.Remove(c);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
