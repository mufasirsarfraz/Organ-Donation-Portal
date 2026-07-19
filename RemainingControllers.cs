using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODMS.Data;
using ODMS.Models;

namespace ODMS.Controllers
{
    // ══════════════════════════════════════════
    // RECIPIENTS
    // ══════════════════════════════════════════
    [ApiController]
    [Route("api/[controller]")]
    public class RecipientsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public RecipientsController(AppDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _db.Recipients.OrderByDescending(r => r.PriorityScore).ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var r = await _db.Recipients.FindAsync(id);
            return r == null ? NotFound() : Ok(r);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Recipient recipient)
        {
            _db.Recipients.Add(recipient);
            await _db.SaveChangesAsync();
            return Ok(recipient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Recipient updated)
        {
            var r = await _db.Recipients.FindAsync(id);
            if (r == null) return NotFound();
            r.FullName         = updated.FullName;
            r.BloodGroup       = updated.BloodGroup;
            r.MedicalCondition = updated.MedicalCondition;
            r.UrgencyLevel     = updated.UrgencyLevel;
            r.WaitlistDate     = updated.WaitlistDate;
            r.PriorityScore    = updated.PriorityScore;
            await _db.SaveChangesAsync();
            return Ok(r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var r = await _db.Recipients.FindAsync(id);
            if (r == null) return NotFound();
            _db.Recipients.Remove(r);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }

    // ══════════════════════════════════════════
    // STAFF
    // ══════════════════════════════════════════
    [ApiController]
    [Route("api/[controller]")]
    public class StaffController : ControllerBase
    {
        private readonly AppDbContext _db;
        public StaffController(AppDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _db.Staff.Include(s => s.Hospital).ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var s = await _db.Staff.Include(x => x.Hospital)
                        .FirstOrDefaultAsync(x => x.StaffID == id);
            return s == null ? NotFound() : Ok(s);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Staff staff)
        {
            _db.Staff.Add(staff);
            await _db.SaveChangesAsync();
            return Ok(staff);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Staff updated)
        {
            var s = await _db.Staff.FindAsync(id);
            if (s == null) return NotFound();
            s.FullName       = updated.FullName;
            s.Role           = updated.Role;
            s.LicenseNumber  = updated.LicenseNumber;
            s.Specialization = updated.Specialization;
            s.Qualifications = updated.Qualifications;
            s.HospitalID     = updated.HospitalID;
            await _db.SaveChangesAsync();
            return Ok(s);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var s = await _db.Staff.FindAsync(id);
            if (s == null) return NotFound();
            _db.Staff.Remove(s);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }

    // ══════════════════════════════════════════
    // MATCH RECORDS
    // ══════════════════════════════════════════
    [ApiController]
    [Route("api/[controller]")]
    public class MatchRecordsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public MatchRecordsController(AppDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _db.MatchRecords
                .Include(m => m.Organ).ThenInclude(o => o!.Donor)
                .Include(m => m.Recipient)
                .ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var m = await _db.MatchRecords
                .Include(x => x.Organ).ThenInclude(o => o!.Donor)
                .Include(x => x.Recipient)
                .FirstOrDefaultAsync(x => x.MatchID == id);
            return m == null ? NotFound() : Ok(m);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MatchRecord match)
        {
            // Mark organ as Matched
            var organ = await _db.Organs.FindAsync(match.OrganID);
            if (organ != null) organ.CurrentStatus = "Matched";

            match.MatchDateTime = DateTime.Now;
            _db.MatchRecords.Add(match);
            await _db.SaveChangesAsync();
            return Ok(match);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] string status)
        {
            var m = await _db.MatchRecords.FindAsync(id);
            if (m == null) return NotFound();
            m.MatchStatus = status;
            await _db.SaveChangesAsync();
            return Ok(m);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var m = await _db.MatchRecords
                .Include(x => x.Organ)
                .FirstOrDefaultAsync(x => x.MatchID == id);
            if (m == null) return NotFound();
            // Revert organ status
            if (m.Organ != null) m.Organ.CurrentStatus = "Available";
            _db.MatchRecords.Remove(m);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }

    // ══════════════════════════════════════════
    // TRANSPLANTS
    // ══════════════════════════════════════════
    [ApiController]
    [Route("api/[controller]")]
    public class TransplantsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public TransplantsController(AppDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _db.Transplants
                .Include(t => t.MatchRecord)
                    .ThenInclude(m => m!.Organ)
                .Include(t => t.Staff)
                .Include(t => t.Hospital)
                .ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var t = await _db.Transplants
                .Include(x => x.MatchRecord)
                .Include(x => x.Staff)
                .Include(x => x.Hospital)
                .FirstOrDefaultAsync(x => x.TransplantID == id);
            return t == null ? NotFound() : Ok(t);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Transplant transplant)
        {
            // Mark organ as Transplanted
            var match = await _db.MatchRecords
                .Include(m => m.Organ)
                .FirstOrDefaultAsync(m => m.MatchID == transplant.MatchID);
            if (match?.Organ != null)
            {
                match.Organ.CurrentStatus = "Transplanted";
                match.MatchStatus = "Completed";
            }
            _db.Transplants.Add(transplant);
            await _db.SaveChangesAsync();
            return Ok(transplant);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Transplant updated)
        {
            var t = await _db.Transplants.FindAsync(id);
            if (t == null) return NotFound();
            t.SurgeryDate     = updated.SurgeryDate;
            t.Outcome         = updated.Outcome;
            t.DurationMinutes = updated.DurationMinutes;
            t.StaffID         = updated.StaffID;
            t.HospitalID      = updated.HospitalID;
            await _db.SaveChangesAsync();
            return Ok(t);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var t = await _db.Transplants.FindAsync(id);
            if (t == null) return NotFound();
            _db.Transplants.Remove(t);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }

    // ══════════════════════════════════════════
    // FOLLOW-UPS
    // ══════════════════════════════════════════
    [ApiController]
    [Route("api/[controller]")]
    public class FollowUpsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public FollowUpsController(AppDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _db.FollowUps.Include(f => f.Transplant).ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var f = await _db.FollowUps.Include(x => x.Transplant)
                        .FirstOrDefaultAsync(x => x.FollowUpNumber == id);
            return f == null ? NotFound() : Ok(f);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FollowUp followUp)
        {
            _db.FollowUps.Add(followUp);
            await _db.SaveChangesAsync();
            return Ok(followUp);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, FollowUp updated)
        {
            var f = await _db.FollowUps.FindAsync(id);
            if (f == null) return NotFound();
            f.ScheduledDate          = updated.ScheduledDate;
            f.RejectionIndicators    = updated.RejectionIndicators;
            f.MedicationPrescribed   = updated.MedicationPrescribed;
            f.TransplantID           = updated.TransplantID;
            await _db.SaveChangesAsync();
            return Ok(f);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var f = await _db.FollowUps.FindAsync(id);
            if (f == null) return NotFound();
            _db.FollowUps.Remove(f);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }

    // ══════════════════════════════════════════
    // WAITING LIST ENTRIES
    // ══════════════════════════════════════════
    [ApiController]
    [Route("api/[controller]")]
    public class WaitingListEntriesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public WaitingListEntriesController(AppDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _db.WaitingListEntries.Include(w => w.Recipient).ToListAsync());

        [HttpPost]
        public async Task<IActionResult> Create(WaitingListEntry entry)
        {
            _db.WaitingListEntries.Add(entry);
            await _db.SaveChangesAsync();
            return Ok(entry);
        }

        [HttpDelete("{entryNumber}/{recipientId}")]
        public async Task<IActionResult> Delete(string entryNumber, string recipientId)
        {
            var w = await _db.WaitingListEntries
                .FirstOrDefaultAsync(x => x.EntryNumber == entryNumber
                                       && x.RecipientID == recipientId);
            if (w == null) return NotFound();
            _db.WaitingListEntries.Remove(w);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }

    // ══════════════════════════════════════════
    // DONATION CAMPAIGNS
    // ══════════════════════════════════════════
    [ApiController]
    [Route("api/[controller]")]
    public class DonationCampaignsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public DonationCampaignsController(AppDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _db.DonationCampaigns.Include(dc => dc.Hospital).ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var dc = await _db.DonationCampaigns.Include(x => x.Hospital)
                        .FirstOrDefaultAsync(x => x.CampaignID == id);
            return dc == null ? NotFound() : Ok(dc);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DonationCampaign campaign)
        {
            _db.DonationCampaigns.Add(campaign);
            await _db.SaveChangesAsync();
            return Ok(campaign);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, DonationCampaign updated)
        {
            var dc = await _db.DonationCampaigns.FindAsync(id);
            if (dc == null) return NotFound();
            dc.CampaignName         = updated.CampaignName;
            dc.StartDate            = updated.StartDate;
            dc.EndDate              = updated.EndDate;
            dc.TargetRegion         = updated.TargetRegion;
            dc.NewDonorsRegistered  = updated.NewDonorsRegistered;
            dc.HospitalID           = updated.HospitalID;
            await _db.SaveChangesAsync();
            return Ok(dc);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var dc = await _db.DonationCampaigns.FindAsync(id);
            if (dc == null) return NotFound();
            _db.DonationCampaigns.Remove(dc);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
