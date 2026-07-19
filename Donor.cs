using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ODMS.Models
{
    [Table("DONOR")]
    public class Donor
    {
        [Key]
        [MaxLength(10)]
        public string DonorID { get; set; } = "";

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = "";

        [Required]
        [MaxLength(15)]
        public string CNIC { get; set; } = "";

        [MaxLength(5)]
        public string? BloodGroup { get; set; }
        // A+ / A- / B+ / B- / AB+ / AB- / O+ / O-

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(20)]
        public string? DonorType { get; set; }
        // Living / Deceased

        [MaxLength(20)]
        public string? DonorStatus { get; set; }
        // Active / Inactive / Pending

        [MaxLength(100)]
        public string? PhoneNumbers { get; set; }

        public int? Age { get; set; }

        // Navigation
        public ICollection<Organ>? Organs { get; set; }
        public ICollection<ConsentRecord>? ConsentRecords { get; set; }
    }
}
