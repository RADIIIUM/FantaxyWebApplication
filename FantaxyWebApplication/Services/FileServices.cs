using FantaxyWebApplication.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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

        public static string CreateFileFromByteArray(IWebHostEnvironment _appEnvironment, byte[] data, string filePath)
        {
            using (var stream = new FileStream(_appEnvironment.WebRootPath + filePath, FileMode.Create))
            {
                stream.Write(data, 0, data.Length);
            }
            return filePath;
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
