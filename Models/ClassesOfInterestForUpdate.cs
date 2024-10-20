using System.ComponentModel.DataAnnotations;

namespace BookInfo.API.Models
{
    public class ClassesOfInterestForUpdate
    {
        [Required]
        [MaxLength(20)]
        public string name { get; set; } = string.Empty;
        [MaxLength(50)]
        public string? description { get; set; }
    }
}
