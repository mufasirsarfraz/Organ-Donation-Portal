using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ODMS.Models
{
    [Table("ORGAN")]
    public class Organ
    {
        [Key]
        [MaxLength(10)]
        public string OrganID { get; set; } = "";

        [Required]
        [MaxLength(10)]
        public string DonorID { get; set; } = "";

        [MaxLength(10)]
        public string? HospitalID { get; set; }

        [MaxLength(50)]
        public string? OrganType { get; set; }
        // Kidney / Liver / Heart / Lungs / Cornea / Pancreas

        public DateTime? HarvestDateTime { get; set; }

        public DateTime? ViabilityExpiryTime { get; set; }

        [MaxLength(30)]
        public string? CurrentStatus { get; set; }
        // Available / Matched / Transplanted / Expired

        public string? SpecialNotes { get; set; }

        [MaxLength(50)]
        public string? CompatibleBloodGroups { get; set; }

        // Navigation
        [ForeignKey("DonorID")]
        public Donor? Donor { get; set; }

        [ForeignKey("HospitalID")]
        public Hospital? Hospital { get; set; }

        public ICollection<OrganPreservation>? Preservations { get; set; }
        public ICollection<MatchRecord>? MatchRecords { get; set; }
    }
}
