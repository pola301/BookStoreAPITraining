using BookInfo.API.DbContexts;
using BookInfo.API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BookInfo.API.Services
{
    public class BookInfoRep : IBookInfoRep
    {
        private readonly BookInfoContext _context;
        public BookInfoRep(BookInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<Book?> GetBookAsync(int Bookid, bool IncClassOfInterest)
        {
            if (IncClassOfInterest == true)
            {
                return await _context.Books.Include(c => c.classesOfInterest).Where(c => c.Id == Bookid).FirstOrDefaultAsync();
            }
            else
            {
                return await _context.Books.Where(c => c.Id == Bookid).FirstOrDefaultAsync();
            }
        }
        public async Task<bool> IsExist(int BookId)
        {
            return await _context.Books.AnyAsync(c => c.Id == BookId);
        }
        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }
        public async Task<(IEnumerable<Book>, MetaData)>GetBooksAsync(string? name,string? searchQuery, int pageNum,int pageSize)
        {
            var collection = _context.Books as IQueryable<Book>;
            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                collection= collection.Where(c => c.Name == name);
            }
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(a => a.Name.Contains(searchQuery) || a.Description != null && a.Description.Contains(searchQuery));
            }
            var totalItems = await collection.CountAsync();
            var metaData = new MetaData(totalItems, pageSize, pageNum);
            var collectionToReturn = await collection
                .Skip(pageSize * (pageNum - 1))
                .Take(pageSize)
                .ToListAsync();
            return (collectionToReturn, metaData);
        }

        public async Task<IEnumerable<ClassOfInterest>> GetClassOfInterestAsync(int Bookid)
        {
            return await _context.classOfInterests.Where(c => c.Id == Bookid).ToListAsync();
        }

        public async Task<ClassOfInterest?> GetClassOfInterestAsync(int Bookid, int Classid)
        {
            return await _context.classOfInterests.Where(c => c.Id == Bookid && c.Id == Classid).FirstOrDefaultAsync();
        }

        public async Task AddClassOfInterestAsync(int bookId, ClassOfInterest classOfInterest)
        {
            var book = await GetBookAsync(bookId, false);
            if (book != null)
            {
                book.classesOfInterest.Add(classOfInterest);
            }
        }
        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public async Task UpdateAsync(int bookId, int classNumber, ClassOfInterest classOfInterest)
        {
            var book = await GetBookAsync(bookId, false);
            if (book != null)
            {
                var classNum = await GetClassOfInterestAsync(bookId, classNumber);
                if (classNum != null)
                {
                    classNum.Name = classOfInterest.Name;
                    classNum.Description = classOfInterest.Description;
                }
            }
        }

        public async Task<bool> IsClassExist(int bookId, int classNumber)
        {
            if (await IsExist(bookId))
            {
                return await _context.classOfInterests.AnyAsync(C=>C.Id == classNumber);
            }
            return false;
        }
        public async Task DeleteAsync(int bookId, int classNumber,ClassOfInterest classOfInterest)
        {
            // First, check if the class exists for the given book
            if (await IsClassExist(bookId, classNumber))
            {
                // Fetch the class to be deleted
                var finalClass = await _context.classOfInterests.FirstOrDefaultAsync(c => c.Id == classNumber);

                // If found, remove it and then save changes
                if (finalClass != null)
                {
                    _context.classOfInterests.Remove(finalClass);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<bool> BookMatches(string? bookName, int bookId)
        {
            return await _context.Books.AnyAsync(c => c.Id == bookId && c.Name == bookName);
        }
    }
}
