using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ODMS.Models
{
    [Table("STAFF")]
    public class Staff
    {
        [Key]
        [MaxLength(10)]
        public string StaffID { get; set; } = "";

        [MaxLength(10)]
        public string? HospitalID { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = "";

        [MaxLength(50)]
        public string? Role { get; set; }
        // Surgeon / Coordinator / Nurse / Admin

        [MaxLength(30)]
        public string? LicenseNumber { get; set; }

        [MaxLength(100)]
        public string? Specialization { get; set; }

        [MaxLength(200)]
        public string? Qualifications { get; set; }

        // Navigation
        [ForeignKey("HospitalID")]
        public Hospital? Hospital { get; set; }

        public ICollection<Transplant>? Transplants { get; set; }
    }
}
