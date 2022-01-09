using System.ComponentModel.DataAnnotations;

namespace Gsuke.ApiPlatform.Models
{
    public class ResourceDto
    {
        [Required]
        [MaxLength(64)]
        [RegularExpression(@"[a-zA-Z0-9]+", ErrorMessage = "Characters are not allowed.")]
        public string? url { get; set; }
        [Required]
        [MaxLength(8192)]
        public string? dataSchema { get; set; }
    }
}
