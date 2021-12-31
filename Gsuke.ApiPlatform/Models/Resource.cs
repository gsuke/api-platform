using System.ComponentModel.DataAnnotations;

namespace Gsuke.ApiPlatform.Models
{
    public class Resource
    {
        [Required]
        public string? Url { get; set; }
        [Required]
        public string? DataSchema { get; set; }
    }
}
