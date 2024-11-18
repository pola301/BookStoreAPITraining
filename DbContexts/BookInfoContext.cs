using BookInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookInfo.API.DbContexts
{
    public class BookInfoContext : DbContext
    {
        // Constructor that accepts DbContextOptions
        public BookInfoContext(DbContextOptions<BookInfoContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<ClassOfInterest> classOfInterests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasData(
                    new Book("48 laws of power")
                    {
                        Id = 1,
                        Description = "Fucking shite",
                        Author = "Ashton Kutcher",
                    },
                    new Book("The Idiot Brain")
                    {
                        Id = 2,
                        Description = "Fucking shite",
                        Author = "Dean Bernat",
                    },
                    new Book("Psychology of the Masses")
                    {
                        Id = 3,
                        Description = "Fucking shite",
                        Author = "Gustave Le Bon",
                    });

            modelBuilder.Entity<ClassOfInterest>()
                .HasData(
                    new ClassOfInterest("Never Outshine the Master")
                    {
                        Id = 1,
                        Description = "Fucking Shite",
                        BookId = 1,
                    },
                    new ClassOfInterest("Long Memory Short Memory")
                    {
                        Id = 2,
                        Description = "Shitee",
                        BookId = 2,
                    },
                    new ClassOfInterest("Reputation Protect It")
                    {
                        Id = 3,
                        Description = "Fucking Shite",
                        BookId = 1,
                    },
                    new ClassOfInterest("Alcohol and Memory")
                    {
                        Id = 4,
                        Description = "Shitee",
                        BookId = 2,
                    },
                    new ClassOfInterest("The Masses are Easy to Manipulate")
                    {
                        Id = 5,
                        Description = "Fucking Shite",
                        BookId = 3,
                    },
                    new ClassOfInterest("Mortada Mansour")
                    {
                        Id = 6,
                        Description = "Shitee",
                        BookId = 3,
                    });

            base.OnModelCreating(modelBuilder);
        }
    }
}
