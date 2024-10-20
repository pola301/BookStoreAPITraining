namespace BookInfo.API.Models
{
    public class BooksDto
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int NumOfClasses
        {
            get
            {
                return classesOfInterest.Count;
            }
        }
        public ICollection<ClassesOfInterest> classesOfInterest { get; set; } = new List<ClassesOfInterest>();
    }
}
