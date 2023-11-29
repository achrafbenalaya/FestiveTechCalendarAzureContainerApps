using Microsoft.AspNetCore.Mvc;
using StorageReader001.Models;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace StorageReader001.Controllers
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
            var imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "upload/images");
            var pdfDirectory = Path.Combine(Directory.GetCurrentDirectory(), "upload/pdf");

            ViewBag.ImageFiles = Directory.EnumerateFiles(imageDirectory)
                                     .Select(x => new FileInfo(x).Name)
                                     .ToList();
            ViewBag.PdfFiles = Directory.EnumerateFiles(pdfDirectory)
                                     .Select(x => new FileInfo(x).Name)
                                     .ToList();

            return View();
        }

        public IActionResult GetImage(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "upload/images", fileName);
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "image/jpeg"); // Change the content type as needed
        }

        public IActionResult GetPdf(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "upload/pdf", fileName);
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/pdf"); // Change the content type as needed
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
