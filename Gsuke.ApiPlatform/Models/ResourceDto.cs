using System.ComponentModel.DataAnnotations;

namespace Gsuke.ApiPlatform.Models
{
    public class ResourceDto
    {
        [Required]
        public string? url { get; set; }
        [Required]
        public string? dataSchema { get; set; }
    }
}
