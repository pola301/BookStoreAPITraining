using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookInfo.API.Entities
{
    public class ClassOfInterest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        // Foreign key reference to Book entity
        public int BookId { get; set; }

        [ForeignKey(nameof(BookId))]  // Corrected attribute to reference the actual foreign key property
        public Book? Books { get; set; }

        public ClassOfInterest(string name)
        {
            Name = name;
        }
    }
}
