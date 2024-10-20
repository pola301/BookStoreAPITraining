using BookInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookInfo.API.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<BooksDto>> GetBooks()
        {
            return Ok(BooksDataStore.Instance.Books);
        }
        [HttpGet("{Id}")]
        public ActionResult<BooksDto> GetBook(int Id)
        {
            var BookIsFound = BooksDataStore.Instance.Books.FirstOrDefault(b => b.Id == Id);
            if (BookIsFound == null) { return NotFound(); }
            return Ok(BookIsFound);
        }
    }
}
