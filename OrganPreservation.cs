using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ODMS.Models
{
    [Table("ORGAN_PRESERVATION")]
    [PrimaryKey(nameof(PreservationNumber), nameof(OrganID))]
    public class OrganPreservation
    {
        [MaxLength(10)]
        public string PreservationNumber { get; set; } = "";

        [Required]
        [MaxLength(10)]
        public string OrganID { get; set; } = "";

        [MaxLength(100)]
        public string? TechnologyName { get; set; }

        public float? StorageTemperature { get; set; }

        public bool MachinePerfusionUsed { get; set; } = false;

        public int? MaxViabilityHours { get; set; }

        // Navigation
        [ForeignKey("OrganID")]
        public Organ? Organ { get; set; }
    }
}
