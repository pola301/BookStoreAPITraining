using BookInfo.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BookInfo.API.Controllers
{
    [Route("api/books/{bookId}/NumClasses")]
    [ApiController]
    public class CLassesOfInterestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<ClassesOfInterest>> GetNumberOfClasses(int bookId)
        {
            var Book = BooksDataStore.Instance.Books.FirstOrDefault(b => b.Id == bookId);
            if (Book == null)
            {
                return NotFound();
            }
            return Ok(Book.NumOfClasses);
        }
        //getting info
        [HttpGet("{classNumber}", Name = "GetNum")]
        public ActionResult<ClassesOfInterest> GetClass(int bookId,int classNumber)
        {
            var Book = BooksDataStore.Instance.Books.FirstOrDefault(b => b.Id == bookId);
            if (Book == null)
            {
                return NotFound();
            }
            var Classes = Book.classesOfInterest.FirstOrDefault(n=> n.id == classNumber);
            if (Classes == null)
            {
                return NotFound();
            }
            return Ok(Classes);
        }
        //Creating on the run
        [HttpPost]
        public ActionResult<ClassesOfInterestForCreationdto> CreateNumOfClasses(int bookId,ClassesOfInterestForCreationdto numOfClasses)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var book = BooksDataStore.Instance.Books.FirstOrDefault(b => b.Id == bookId);
            if (book == null)
            {
                return NotFound();
            }
            var maxClassesOfInterest = BooksDataStore.Instance.Books.SelectMany(b=>b.classesOfInterest).Max(m=>m.id);

            var finalClassesOfInterest = new ClassesOfInterest()
            {
                id = ++maxClassesOfInterest,
                name = numOfClasses.name,
                description = numOfClasses.description
            };
            book.classesOfInterest.Add(finalClassesOfInterest);
            return CreatedAtRoute("GetNum", new { bookId = bookId, classNumber = finalClassesOfInterest.id}, finalClassesOfInterest);
        }
        //Updating برضه
        [HttpPut("{classNumber}")]
        public ActionResult UpdateClassOfInterest(int bookId,int classNumber,ClassesOfInterestForUpdate classesOfInterestForUpdate)
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
            ClassOfInterest.name = classesOfInterestForUpdate.name;
            ClassOfInterest.description = classesOfInterestForUpdate.description;
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
        public ActionResult DeleteClass(int bookId, int classNumber)
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
            book.classesOfInterest.Remove(ClassOfInterest);
            return NoContent();
        }
    }
}
