using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebSwitchFileRenamingWorking.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnPost(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    byte[] fileData = memoryStream.ToArray();

                    // Your C# logic here, you can process the fileData as needed
                    // For example, save it to the database, perform some operations, etc.
                }

                return new JsonResult("File received successfully.");
            }

            return BadRequest("No file received.");
        }
    }
}