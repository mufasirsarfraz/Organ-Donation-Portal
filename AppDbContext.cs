using Microsoft.EntityFrameworkCore;
using ODMS.Models;

namespace ODMS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // ── All 12 tables ──
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<Donor> Donors { get; set; }
        public DbSet<ConsentRecord> ConsentRecords { get; set; }
        public DbSet<Organ> Organs { get; set; }
        public DbSet<OrganPreservation> OrganPreservations { get; set; }
        public DbSet<Recipient> Recipients { get; set; }
        public DbSet<WaitingListEntry> WaitingListEntries { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<MatchRecord> MatchRecords { get; set; }
        public DbSet<Transplant> Transplants { get; set; }
        public DbSet<FollowUp> FollowUps { get; set; }
        public DbSet<DonationCampaign> DonationCampaigns { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Composite PK for weak entity: ORGAN_PRESERVATION
            modelBuilder.Entity<OrganPreservation>()
                .HasKey(op => new { op.PreservationNumber, op.OrganID });

            // Composite PK for weak entity: WAITING_LIST_ENTRY
            modelBuilder.Entity<WaitingListEntry>()
                .HasKey(w => new { w.EntryNumber, w.RecipientID });

            // DONOR → ORGAN (one donor can donate many organs)
            modelBuilder.Entity<Organ>()
                .HasOne(o => o.Donor)
                .WithMany(d => d.Organs)
                .HasForeignKey(o => o.DonorID)
                .OnDelete(DeleteBehavior.Restrict);

            // HOSPITAL → ORGAN
            modelBuilder.Entity<Organ>()
                .HasOne(o => o.Hospital)
                .WithMany(h => h.Organs)
                .HasForeignKey(o => o.HospitalID)
                .OnDelete(DeleteBehavior.SetNull);

            // DONOR → CONSENT_RECORD
            modelBuilder.Entity<ConsentRecord>()
                .HasOne(c => c.Donor)
                .WithMany(d => d.ConsentRecords)
                .HasForeignKey(c => c.DonorID)
                .OnDelete(DeleteBehavior.Cascade);

            // ORGAN → ORGAN_PRESERVATION
            modelBuilder.Entity<OrganPreservation>()
                .HasOne(op => op.Organ)
                .WithMany(o => o.Preservations)
                .HasForeignKey(op => op.OrganID)
                .OnDelete(DeleteBehavior.Cascade);

            // RECIPIENT → WAITING_LIST_ENTRY
            modelBuilder.Entity<WaitingListEntry>()
                .HasOne(w => w.Recipient)
                .WithMany(r => r.WaitingListEntries)
                .HasForeignKey(w => w.RecipientID)
                .OnDelete(DeleteBehavior.Cascade);

            // HOSPITAL → STAFF
            modelBuilder.Entity<Staff>()
                .HasOne(s => s.Hospital)
                .WithMany(h => h.Staff)
                .HasForeignKey(s => s.HospitalID)
                .OnDelete(DeleteBehavior.SetNull);

            // ORGAN → MATCH_RECORD
            modelBuilder.Entity<MatchRecord>()
                .HasOne(m => m.Organ)
                .WithMany(o => o.MatchRecords)
                .HasForeignKey(m => m.OrganID)
                .OnDelete(DeleteBehavior.SetNull);

            // RECIPIENT → MATCH_RECORD
            modelBuilder.Entity<MatchRecord>()
                .HasOne(m => m.Recipient)
                .WithMany(r => r.MatchRecords)
                .HasForeignKey(m => m.RecipientID)
                .OnDelete(DeleteBehavior.SetNull);

            // MATCH_RECORD → TRANSPLANT
            modelBuilder.Entity<Transplant>()
                .HasOne(t => t.MatchRecord)
                .WithOne(m => m.Transplant)
                .HasForeignKey<Transplant>(t => t.MatchID)
                .OnDelete(DeleteBehavior.SetNull);

            // STAFF → TRANSPLANT
            modelBuilder.Entity<Transplant>()
                .HasOne(t => t.Staff)
                .WithMany(s => s.Transplants)
                .HasForeignKey(t => t.StaffID)
                .OnDelete(DeleteBehavior.SetNull);

            // HOSPITAL → TRANSPLANT
            modelBuilder.Entity<Transplant>()
                .HasOne(t => t.Hospital)
                .WithMany(h => h.Transplants)
                .HasForeignKey(t => t.HospitalID)
                .OnDelete(DeleteBehavior.SetNull);

            // TRANSPLANT → FOLLOW_UP
            modelBuilder.Entity<FollowUp>()
                .HasOne(f => f.Transplant)
                .WithMany(t => t.FollowUps)
                .HasForeignKey(f => f.TransplantID)
                .OnDelete(DeleteBehavior.SetNull);

            // HOSPITAL → DONATION_CAMPAIGN
            modelBuilder.Entity<DonationCampaign>()
                .HasOne(dc => dc.Hospital)
                .WithMany(h => h.DonationCampaigns)
                .HasForeignKey(dc => dc.HospitalID)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}