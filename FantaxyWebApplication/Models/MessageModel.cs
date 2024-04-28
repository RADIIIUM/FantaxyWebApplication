namespace FantaxyWebApplication.Models
{
    public class MessageModel
    {
        public string Owner { get; set; }
        public string Avatar { get; set; }
        public string message { get; set; }

        public bool IsAuthor { get; set; } = false;
    }
}
