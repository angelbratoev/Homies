using System.ComponentModel.DataAnnotations;
using static Homies.Data.DataConstants;
using static Homies.Data.ViewModelErrors;

namespace Homies.Models
{
    public class EventFormViewModel
    {
        [Required(ErrorMessage = RequiredField)]
        [StringLength(EventNameMaxLength,
            MinimumLength = EventNameMinLength,
            ErrorMessage = IncorrectLength)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = RequiredField)]
        [StringLength(EventDescriptionMaxLength,
            MinimumLength = EventDescriptionMinLength,
            ErrorMessage = IncorrectLength)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = RequiredField)]
        public string Start { get; set; } = string.Empty;

        [Required(ErrorMessage = RequiredField)]
        public string End { get; set; } = string.Empty;

        [Required(ErrorMessage = RequiredField)]
        public int TypeId { get; set; }

        public IEnumerable<TypeViewModel> Types { get; set; } = new List<TypeViewModel>();
    }
}
