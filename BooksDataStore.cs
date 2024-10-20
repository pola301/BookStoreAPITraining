using BookInfo.API.Models;

namespace BookInfo.API
{
    public class BooksDataStore
    {
        public List<BooksDto> Books { get; set; }
        public static BooksDataStore Instance { get; } = new BooksDataStore();
        public BooksDataStore() { 
            Books = new List<BooksDto>() {
                new BooksDto()
                {
                    Id = 1,
                    name = "The Idiot brain",
                    description = "Description",
                    classesOfInterest = new List<ClassesOfInterest>()
                    {
                        new ClassesOfInterest()
                        {
                            id = 1,
                            name= "The alcohol and the brain",
                            description = "Description",
                        },
                        new ClassesOfInterest()
                        {
                            id= 2,
                            name= "Short memory long memory",
                            description = "Description",
                        }
                    }

                },
                new BooksDto()
                {
                    Id= 2,
                    name= "48 Laws of power",
                    description = "Description",
                    classesOfInterest = new List<ClassesOfInterest>()
                    {
                        new ClassesOfInterest()
                        {
                            id = 1,
                            name= "Never OutShine the master",
                            description = "Description",
                        },
                        new ClassesOfInterest()
                        {
                            id= 2,
                            name= "Fuck You",
                            description = "Description",
                        }
                    }
                }
            };
        }
    }
}
