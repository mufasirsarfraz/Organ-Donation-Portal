using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ODMS.Models
{
    [Table("DONATION_CAMPAIGN")]
    public class DonationCampaign
    {
        [Key]
        [MaxLength(10)]
        public string CampaignID { get; set; } = "";

        [MaxLength(10)]
        public string? HospitalID { get; set; }

        [MaxLength(150)]
        public string? CampaignName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [MaxLength(100)]
        public string? TargetRegion { get; set; }

        public int NewDonorsRegistered { get; set; } = 0;

        // Navigation
        [ForeignKey("HospitalID")]
        public Hospital? Hospital { get; set; }
    }
}
