using Microsoft.AspNetCore.Mvc;
using MetadataExtractor;
using PhotoVerification.Services.ImageMetadataService;

namespace PhotoVerification.Controllers
{
    public class PhotoController : Controller
    {
        private readonly IImageMetadataService _metadataService;

        public PhotoController(IImageMetadataService metadataService)
        {
            _metadataService = metadataService;
        }

        // GET: Photo/Upload
        public ActionResult Upload()
        {
            return View();
        }

        // POST: Photo/Upload
        [HttpPost]
        public ActionResult Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Message"] = "Файл не был загружен";
                return RedirectToAction("Index", "Home");
            }

            string[] allowedExtensions = [".png", ".jpg"];
            var fileExtension = Path.GetExtension(file.FileName);

            if (!allowedExtensions.Contains(fileExtension, StringComparer.InvariantCultureIgnoreCase))
            {
                TempData["Message"] = "Разрешено загружать только файлы с расширением .png и .jpg";
                return RedirectToAction("Index", "Home");
            }

            try
            {
                byte[]? imageData = null;

                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    imageData = memoryStream.ToArray();
                }

                var directories = ImageMetadataReader.ReadMetadata(new MemoryStream(imageData));
                string message = _metadataService.ProcessMetadata(directories);
                TempData["Message"] = $"Файл '{file.FileName}' {message}";
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Ошибка при обработке файла: {ex.Message}";
            }


            return RedirectToAction("Index", "Home");
        }
    }
}
