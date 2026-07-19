using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ODMS.Models
{
    [Table("CONSENT_RECORD")]
    public class ConsentRecord
    {
        [Key]
        [MaxLength(10)]
        public string ConsentID { get; set; } = "";

        [Required]
        [MaxLength(10)]
        public string DonorID { get; set; } = "";

        public DateTime? ConsentDate { get; set; }

        [MaxLength(50)]
        public string? ConsentType { get; set; }
        // Full / Partial / Specific

        [MaxLength(100)]
        public string? WitnessName { get; set; }

        public bool CNICVerified { get; set; } = false;

        // Navigation
        [ForeignKey("DonorID")]
        public Donor? Donor { get; set; }
    }
}
