using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ODMS.Models
{
    [Table("MATCH_RECORD")]
    public class MatchRecord
    {
        [Key]
        [MaxLength(10)]
        public string MatchID { get; set; } = "";

        [MaxLength(10)]
        public string? OrganID { get; set; }

        [MaxLength(10)]
        public string? RecipientID { get; set; }

        public DateTime? MatchDateTime { get; set; }

        public float? MatchScore { get; set; }
        // 0.0 to 100.0

        [MaxLength(30)]
        public string? MatchStatus { get; set; }
        // Pending / Approved / Rejected / Completed

        [MaxLength(10)]
        public string? AlgorithmVersion { get; set; }

        // Navigation
        [ForeignKey("OrganID")]
        public Organ? Organ { get; set; }

        [ForeignKey("RecipientID")]
        public Recipient? Recipient { get; set; }

        public Transplant? Transplant { get; set; }
    }
}
