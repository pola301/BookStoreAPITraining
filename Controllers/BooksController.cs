using Asp.Versioning;
using AutoMapper;
using BookInfo.API.Models;
using BookInfo.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BookInfo.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v{version:apiVersion}/books")]
    [ApiVersion(2)]
    public class BooksController : ControllerBase
    {
        private readonly IBookInfoRep _bookInfoRep1;
        private readonly IMapper _mapper;
        const int maxPageSize = 20;

        public BooksController(IBookInfoRep bookInfoRep, IMapper mapper) {
            _bookInfoRep1 = bookInfoRep ?? throw new ArgumentNullException();
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        /// <summary>
        /// Getting all the books in system data
        /// </summary>
        /// <param name="name">Get Books</param>
        /// <param name="searchQuery"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BooksWithoutClassesOfInterest>>> GetBooks(string? name,string? searchQuery, int pageNum = 1, int pageSize = 10)
        {
            if (pageSize > maxPageSize)
            {
                pageNum = maxPageSize;
            }
            var (books,metaData) = await _bookInfoRep1.GetBooksAsync(name,searchQuery,pageNum,pageSize);
            Response.Headers.Add("X-paginztion",JsonSerializer.Serialize(metaData));
            return Ok(_mapper.Map<IEnumerable<BooksWithoutClassesOfInterest>>(books));
        }
        /// <summary>
        /// Gettimg a specific book
        /// </summary>
        /// <param name="bookId"> with this ID</param>
        /// <param name="IncClassesOfInterest"> and if it include classes of interest or no</param>
        /// <returns></returns>
        [HttpGet("{bookId}")]
        public async Task<IActionResult> GetBook(int bookId, bool IncClassesOfInterest = false)
        {
            var book = await _bookInfoRep1.GetBookAsync(bookId,IncClassesOfInterest);
            if (book == null) {
                return NotFound();
            }
            if (IncClassesOfInterest == false)
            {
                return Ok(_mapper.Map<BooksWithoutClassesOfInterest>(book));
            }
            else {
                return Ok(_mapper.Map<BooksDto>(book));
            }
        }
    }
}
