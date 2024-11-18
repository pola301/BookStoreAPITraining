using BookInfo.API.Entities;

namespace BookInfo.API.Services
{
    public interface IBookInfoRep
    {
        Task<IEnumerable<Book>> GetBooksAsync();
        Task<(IEnumerable<Book>,MetaData)> GetBooksAsync(string? name, string? searchQuery,int pageNum,int pageSize);
        Task<Book?> GetBookAsync(int Bookid, bool IncClassOfInterest);
        Task<IEnumerable<ClassOfInterest>> GetClassOfInterestAsync(int Bookid);
        Task<ClassOfInterest> GetClassOfInterestAsync(int Bookid,int Classid);
        Task<bool> IsExist(int bookId);
        Task AddClassOfInterestAsync(int bookId,ClassOfInterest classOfInterest);
        Task UpdateAsync(int bookId,int classNumber,ClassOfInterest classOfInterest);
        Task<bool> IsClassExist(int bookId,int classNumber);
        Task<bool> SaveAsync();
        Task<bool> BookMatches(string? bookName,int bookId);
        Task DeleteAsync(int bookId,int classNumber,ClassOfInterest classOfInterest);
    }
}
