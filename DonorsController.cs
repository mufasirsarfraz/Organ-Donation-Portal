using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODMS.Data;
using ODMS.Models;

namespace ODMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DonorsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public DonorsController(AppDbContext db) { _db = db; }

        // GET all donors
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _db.Donors.ToListAsync());

        // GET donor by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var d = await _db.Donors.FindAsync(id);
            return d == null ? NotFound() : Ok(d);
        }

        // POST add new donor
        [HttpPost]
        public async Task<IActionResult> Create(Donor donor)
        {
            _db.Donors.Add(donor);
            await _db.SaveChangesAsync();
            return Ok(donor);
        }

        // PUT update donor
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Donor updated)
        {
            var d = await _db.Donors.FindAsync(id);
            if (d == null) return NotFound();
            d.FullName    = updated.FullName;
            d.CNIC        = updated.CNIC;
            d.BloodGroup  = updated.BloodGroup;
            d.DateOfBirth = updated.DateOfBirth;
            d.DonorType   = updated.DonorType;
            d.DonorStatus = updated.DonorStatus;
            d.PhoneNumbers= updated.PhoneNumbers;
            d.Age         = updated.Age;
            await _db.SaveChangesAsync();
            return Ok(d);
        }

        // DELETE donor
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var d = await _db.Donors.FindAsync(id);
            if (d == null) return NotFound();
            _db.Donors.Remove(d);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
