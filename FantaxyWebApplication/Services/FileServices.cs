using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace FantaxyWebApplication.Services
{
    public static class FileServices
    {
        
        public static IFormFile ConvertFileToIFormFile(IWebHostEnvironment appEnvironment, string filePath)
        {
            var fileInfo = appEnvironment.WebRootFileProvider.GetFileInfo(filePath);
            var stream = fileInfo.CreateReadStream();
            var fileName = fileInfo.Name;

            return new FormFile(stream, 0, stream.Length, fileName, fileName);
        }

        public static IFormFile UploadFile (IWebHostEnvironment appEnvironment, string filePath)
        {
            var fileInfo = appEnvironment.WebRootFileProvider.GetFileInfo(filePath);
            var stream = fileInfo.CreateReadStream();
            var fileName = fileInfo.Name;

            return new FormFile(stream, 0, stream.Length, fileName, fileName);
        }

    }
}
