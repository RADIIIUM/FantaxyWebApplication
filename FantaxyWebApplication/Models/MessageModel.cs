namespace FantaxyWebApplication.Models
{
    public class MessageModel
    {
        public string owner { get; set; }
        public string avatar { get; set; }
        public string message { get; set; }

        public bool IsAuthor { get; set; } = false;
    }
}
