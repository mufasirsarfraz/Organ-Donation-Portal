using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ODMS.Models
{
    [Table("HOSPITAL")]
    public class Hospital
    {
        [Key]
        [MaxLength(10)]
        public string HospitalID { get; set; } = "";

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = "";

        [MaxLength(150)]
        public string? Location { get; set; }

        [MaxLength(50)]
        public string? HospitalCategory { get; set; }
        // Teaching / Specialized / General

        public int? ICUCapacity { get; set; }

        // Navigation
        public ICollection<Staff>? Staff { get; set; }
        public ICollection<Transplant>? Transplants { get; set; }
        public ICollection<DonationCampaign>? DonationCampaigns { get; set; }
        public ICollection<Organ>? Organs { get; set; }
    }
}
