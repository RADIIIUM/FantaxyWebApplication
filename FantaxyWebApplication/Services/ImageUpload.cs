using FantaxyWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace FantaxyWebApplication.Services
{
    public static class ImageUpload
    {

        public static byte[] UploadImage(IFormFile Avatar)
        {
            return ConvertIFormFileToByteArray(Avatar);
        }

        public static byte[] UploadPostImage (this IHttpContextAccessor httpContextAccessor, IFormFile file)
        {
            byte[] avatar = ImageUpload.UploadImage(file);
            return avatar;
        }

        public static byte[] UploadProfileImage(this IHttpContextAccessor httpContextAccessor, IFormFile file, string session, EditModel model, string tag)
        {
            byte[] avatar = ImageUpload.UploadImage(file);
            if(tag == "_A") model.Avatar = avatar;
            if (tag == "_M") model.Main = avatar;
            if (tag == "_P") model.Profile = avatar;
            httpContextAccessor.HttpContext.Session.Remove(session);
            httpContextAccessor.HttpContext.Session.Set<EditModel>(session, model);
            return avatar;
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
