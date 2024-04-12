using Microsoft.AspNetCore.Mvc;

namespace FantaxyWebApplication.Services
{
    public static class ImageUpload
    {
        public static byte[] UploadImage(IFormFile Avatar)
        {
            return ConvertIFormFileToByteArray(Avatar);
        }

        public static string NameCut(string name, int maxLenght)
        {
            if (name.Length >= maxLenght)
            {
                name = $"{name.Substring(0, 10)}...";
                return name;
            }
            else
            {
                return name;
            }
        }
        public static byte[] ConvertIFormFileToByteArray(IFormFile file)
        {
            if(file == null)
            {
                return null;
            }
            using (MemoryStream memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
