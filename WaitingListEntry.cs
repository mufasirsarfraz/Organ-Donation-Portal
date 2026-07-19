using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ODMS.Models
{
    [Table("WAITING_LIST_ENTRY")]
    [PrimaryKey(nameof(EntryNumber), nameof(RecipientID))]
    public class WaitingListEntry
    {
        [MaxLength(10)]
        public string EntryNumber { get; set; } = "";

        [Required]
        [MaxLength(10)]
        public string RecipientID { get; set; } = "";

        public DateTime? DateAdded { get; set; }

        public int? EstimatedWaitDays { get; set; }

        [MaxLength(20)]
        public string? Priority { get; set; }
        // High / Medium / Low

        // Navigation
        [ForeignKey("RecipientID")]
        public Recipient? Recipient { get; set; }
    }
}
