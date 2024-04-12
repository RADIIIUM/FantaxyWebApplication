using FantaxyWebApplication.Models.Entities;
using System.Security.Claims;
using System.Text.Json;

namespace FantaxyWebApplication.Services
{
    public static class CookieManager
    {
        public static void SetUserModelCookie<T>(this HttpResponse response, string key, T userModel)
        {
            var serializedUserModel = JsonSerializer.Serialize(userModel);
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(7),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };
            response.Cookies.Append($"{key}", serializedUserModel, cookieOptions);
        }


        public static T GetUserModelCookie<T>(this HttpRequest request, string key)
        {
            var cookieValue = request.Cookies[$"{key}"];
            if (string.IsNullOrEmpty(cookieValue))
            {
                return default(T);
            }
            var deserializedUserModel = JsonSerializer.Deserialize<T>(cookieValue);
            return deserializedUserModel;
        }
    }
}
