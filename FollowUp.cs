using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ODMS.Models
{
    [Table("FOLLOW_UP")]
    public class FollowUp
    {
        [Key]
        [MaxLength(10)]
        public string FollowUpNumber { get; set; } = "";

        [MaxLength(10)]
        public string? TransplantID { get; set; }

        public DateTime? ScheduledDate { get; set; }

        public string? RejectionIndicators { get; set; }

        public string? MedicationPrescribed { get; set; }

        // Navigation
        [ForeignKey("TransplantID")]
        public Transplant? Transplant { get; set; }
    }
}
