using System.ComponentModel.DataAnnotations;

namespace BookInfo.API.Models
{
    public class ClassesOfInterestForCreationdto
    {
        [Required]
        [MaxLength(50)]
        public string name { get; set; } = string.Empty;
        [MaxLength(250)]
        public string? description { get; set; }
    }
}
