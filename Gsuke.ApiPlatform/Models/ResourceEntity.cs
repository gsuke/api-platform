using System.ComponentModel.DataAnnotations;

namespace Gsuke.ApiPlatform.Models
{
    public class ResourceEntity
    {
        [Required]
        public string? url { get; set; }
        [Required]
        public string? data_schema { get; set; }
    }
}
