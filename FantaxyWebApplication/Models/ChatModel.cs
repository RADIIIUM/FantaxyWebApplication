namespace FantaxyWebApplication.Models
{
    public class ChatModel
    {
        public int IdChat { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public int MembersCount { get; set; }
        public string MainBackground { get; set; }
        public string ProfileBackground { get; set; }
        public string OwnerLogin { get; set; }
        public int IdPlanet { get; set; }
    }
}
