using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace BookInfo.API.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class Files : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider _extensionContentTypeProvider;
        public Files(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _extensionContentTypeProvider = fileExtensionContentTypeProvider
            ?? throw new System.ArgumentException(
                nameof(fileExtensionContentTypeProvider));
        }
        //Getting a file downloading
        [HttpGet("{fileId}")]
        public ActionResult GetFile(string fileId)
        {
            var pathToFile = "MY CV.pdf";
            if (!System.IO.File.Exists(pathToFile))
            {
                return NotFound();
            }
            if (!_extensionContentTypeProvider.TryGetContentType(pathToFile, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            var bytes = System.IO.File.ReadAllBytes(pathToFile);
            return File(bytes, contentType, Path.GetFileName(pathToFile));
        }
        //Uploading a file Temp
        [HttpPost]
        public async Task<ActionResult> GetFile(IFormFile file)
        {
            if (file.Length == 0 || file.Length > 20000000 || file.ContentType != "application/pdf")
            {
                return BadRequest("Large or no file asln");
            }
            var path = Path.Combine(Directory.GetCurrentDirectory(),$"uploaded_file_{Guid.NewGuid()}.pdf");
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
                return Ok("file ready");
        }
    }
}
