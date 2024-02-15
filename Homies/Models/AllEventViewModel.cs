using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace Homies.Models
{
    public class AllEventViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Organiser { get; set; } = string.Empty;

        [Required]
        public string Start { get; set; } = string.Empty;

        [Required]
        public string Type { get; set; } = string.Empty;
    }
}
