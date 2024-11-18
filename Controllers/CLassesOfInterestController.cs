using Asp.Versioning;
using AutoMapper;
using BookInfo.API.Models;
using BookInfo.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace BookInfo.API.Controllers
{
    [Route("api/v{version:apiVersion}/books/{bookId}/NumClasses")]
    [Authorize(Policy = "must be from antwerp")]
    [ApiController]
    [ApiVersion(2)]
    public class CLassesOfInterestController : ControllerBase
    {
        private readonly ILogger<CLassesOfInterestController> _logger;
        private readonly IMail _mail;
        private readonly IMapper _mapper;
        private readonly IBookInfoRep _bookInfoRep;

        public CLassesOfInterestController(ILogger<CLassesOfInterestController> logger,IMail mail,IBookInfoRep bookInfoRep,IMapper mapper){
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mail = mail ?? throw new ArgumentNullException(nameof(mail));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _bookInfoRep = bookInfoRep ?? throw new ArgumentNullException(nameof(bookInfoRep));
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassesOfInterest>>> GetNumberOfClasses(int bookId)
        {
            var bookName = User.Claims.FirstOrDefault(c => c.Type == "book")?.Value;
            if (!(await _bookInfoRep.BookMatches(bookName, bookId))){
                return Forbid();
            }
            if (!await _bookInfoRep.IsExist(bookId))
            {
                _logger.LogInformation($"book with id {bookId} is not found");
                return NotFound();
            }
            var classOfInterestForBook = await _bookInfoRep.GetClassOfInterestAsync(bookId);

            return Ok(_mapper.Map<IEnumerable<ClassesOfInterest>>(classOfInterestForBook));
        }
        //getting info
        [HttpGet("{classNumber}", Name = "GetNum")]
        public async Task<ActionResult<ClassesOfInterest>> GetClass(int bookId,int classNumber)
        {
            if (!await _bookInfoRep.IsExist(bookId))
            {
                _logger.LogInformation($"book with id {bookId} is not found");
                return NotFound();
            }
            var classOfInterestForBook = await _bookInfoRep.GetClassOfInterestAsync(bookId, classNumber);
            if (classOfInterestForBook == null)
            {
                _logger.LogInformation($"book class of interest not found with id {bookId} and class {classNumber}");
                return NotFound();
            }
           
            return Ok(_mapper.Map<ClassesOfInterest>(classOfInterestForBook));

        }
        //Creating on the run
        [HttpPost("{classNumber}")]
        public async Task<ActionResult<ClassesOfInterest>> CreateNumOfClasses(int bookId,ClassesOfInterestForCreationdto numOfClasses)
        {
            if (!await _bookInfoRep.IsExist(bookId))
            {
                return NotFound();
            }
            var finalClassOfInterest = _mapper.Map<Entities.ClassOfInterest>(numOfClasses);
            await _bookInfoRep.AddClassOfInterestAsync(bookId, finalClassOfInterest);
            await _bookInfoRep.SaveAsync();
            var createdClass = _mapper.Map<Models.ClassesOfInterest>(finalClassOfInterest);
            return CreatedAtRoute("GetNum", new { bookId = bookId, classNumber = createdClass.id}, createdClass);
        }
        //Updating برضه
        [HttpPut("{classNumber}")]
        public async Task<ActionResult> UpdateClassOfInterest(int bookId,int classNumber,ClassesOfInterestForUpdate classesOfInterestForUpdate)
        {
            if (!await _bookInfoRep.IsExist(bookId))
            {
                return NotFound();
            }
            if (!await _bookInfoRep.IsClassExist(bookId,classNumber))
            {
                return NotFound();
            }
            var finalClass = _mapper.Map<Entities.ClassOfInterest>(classesOfInterestForUpdate);
            await _bookInfoRep.UpdateAsync(bookId, classNumber, finalClass);
            await _bookInfoRep.SaveAsync();
            var Updated = _mapper.Map<Models.ClassesOfInterest>(finalClass);
            return NoContent();
        }
        //Updating
        [HttpPatch("{classNumber}")]
        public ActionResult PartiallyUpdate(int bookId,int classNumber,JsonPatchDocument<ClassesOfInterestForUpdate> UpdatePartially)
        {
            var book = BooksDataStore.Instance.Books.FirstOrDefault(b => b.Id == bookId);
            if (book == null)
            {
                return NotFound();
            }
            var ClassOfInterest = book.classesOfInterest.FirstOrDefault(c => c.id == classNumber);
            if (ClassOfInterest == null)
            {
                return NotFound();
            }
            var classOfInterestForPatch = new ClassesOfInterestForUpdate(){ name = ClassOfInterest.name,
            description = ClassOfInterest.description};
            UpdatePartially.ApplyTo(classOfInterestForPatch, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (!TryValidateModel(classOfInterestForPatch))
            {
                return BadRequest(ModelState);
            }
            ClassOfInterest.name = classOfInterestForPatch.name;
            ClassOfInterest.description = classOfInterestForPatch.description;

            return NoContent();
        }
        //Deleting anything
        [HttpDelete("{classNumber}")]
        public async Task<ActionResult> DeleteClass(int bookId, int classNumber,ClassesOfInterest classesOfInterest)
        {
            if (!await _bookInfoRep.IsClassExist(bookId,classNumber))
            {
                return NotFound();
            }
            var finalC = _mapper.Map<Entities.ClassOfInterest>(classesOfInterest);
            await _bookInfoRep.DeleteAsync(bookId, classNumber,finalC);
            await _bookInfoRep.SaveAsync();
            _mail.send($"Class of interest is Deleted " +
                $"Class of interest {finalC.Name} , Class of interest ID {finalC.Id}");
            return NoContent();
        }
    }
}
