using Microsoft.AspNetCore.Mvc;
using StorageWriter.Models;
using System.Diagnostics;

namespace StorageWriter.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile upload)
        {
            if (upload != null && upload.Length > 0)
            {
                var fileName = Path.GetFileName(upload.FileName);
                var directory = Path.Combine(Directory.GetCurrentDirectory(), "upload");

                // Check if the file is a PDF
                if (Path.GetExtension(fileName).Equals(".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    directory = Path.Combine(directory, "pdf");
                }
                // Check if the file is an image (you can customize this based on your accepted image file extensions)
                else if (IsImageFile(fileName))
                {
                    directory = Path.Combine(directory, "images");
                }

                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                var filePath = Path.Combine(directory, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await upload.CopyToAsync(fileStream);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        // Helper function to check if the file has an image extension
        private bool IsImageFile(string fileName)
        {
            string[] allowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            string fileExtension = Path.GetExtension(fileName).ToLower();
            return allowedImageExtensions.Contains(fileExtension);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}