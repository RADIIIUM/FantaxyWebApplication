namespace FantaxyWebApplication.Models
{
    public class EditModel
    {
        public EditModel() { }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }

        public string Description { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }

        public byte[] Avatar { get; set; }

        public byte[] Main { get; set; }

        public byte[] Profile { get; set; }
    }
}
