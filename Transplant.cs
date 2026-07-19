using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ODMS.Models
{
    [Table("TRANSPLANT")]
    public class Transplant
    {
        [Key]
        [MaxLength(10)]
        public string TransplantID { get; set; } = "";

        [MaxLength(10)]
        public string? MatchID { get; set; }

        [MaxLength(10)]
        public string? StaffID { get; set; }

        [MaxLength(10)]
        public string? HospitalID { get; set; }

        public DateTime? SurgeryDate { get; set; }

        [MaxLength(50)]
        public string? Outcome { get; set; }
        // Successful / Failed / Ongoing

        public int? DurationMinutes { get; set; }

        // Navigation
        [ForeignKey("MatchID")]
        public MatchRecord? MatchRecord { get; set; }

        [ForeignKey("StaffID")]
        public Staff? Staff { get; set; }

        [ForeignKey("HospitalID")]
        public Hospital? Hospital { get; set; }

        public ICollection<FollowUp>? FollowUps { get; set; }
    }
}
