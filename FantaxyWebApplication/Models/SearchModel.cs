using FantaxyWebApplication.Models.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FantaxyWebApplication.Models
{
    public class SearchModel : PageModel
    {
        private readonly FantaxyContext _context;

        public SearchModel()
        {

        }
        public SearchModel(string id, string name, int roleStatus, string ava, string prf)
        {
            this.Id = id;
            this.Name = name;
            this.RoleOrStatus = roleStatus;
            this.Avatar = ava;
            this.Profile = prf;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public int RoleOrStatus { get; set; }
        public string Avatar { get; set; }
        public string Profile { get; set; }

    }
}
