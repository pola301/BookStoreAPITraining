using BookInfo.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookInfo.API.Entities
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        public string Author { get; set; }
        public ICollection<ClassOfInterest> classesOfInterest { get; set; } = new List<ClassOfInterest>();
        public int NumOfClasses
        {
            get
            {
                return classesOfInterest.Count;
            }
        }
        public Book(string name){
            Name = name;
        }
    }
}
