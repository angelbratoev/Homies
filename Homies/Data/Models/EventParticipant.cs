using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace Homies.Data.Models
{
    public class EventParticipant
    {
        [Required]
        public string HelperId { get; set; } = string.Empty;

        [Required]
        [ForeignKey(nameof(HelperId))]
        public IdentityUser Helper { get; set; } = null!;

        [Required]
        public int EventId { get; set; }

        [Required]
        [ForeignKey(nameof(EventId))]
        public Event Event { get; set; } = null!;
    }
}
