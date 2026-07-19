using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ODMS.Models
{
    [Table("RECIPIENT")]
    public class Recipient
    {
        [Key]
        [MaxLength(10)]
        public string RecipientID { get; set; } = "";

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = "";

        [MaxLength(5)]
        public string? BloodGroup { get; set; }

        [MaxLength(150)]
        public string? MedicalCondition { get; set; }

        [MaxLength(20)]
        public string? UrgencyLevel { get; set; }
        // Critical / High / Medium / Low

        public DateTime? WaitlistDate { get; set; }

        public int? PriorityScore { get; set; }

        // Navigation
        public ICollection<WaitingListEntry>? WaitingListEntries { get; set; }
        public ICollection<MatchRecord>? MatchRecords { get; set; }
    }
}
