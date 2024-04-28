namespace FantaxyWebApplication.Models
{
    public class ChatCreate
    {
        public int IdChat { get; set; }
        public string Name { get; set; }
        public string? Desc { get; set; } = "";
        public byte[]? ProfileBack { get; set; }
        public byte[]? MainBackground { get; set; }
        public int? IdPlanet { get; set; }
        public string OwnerLogin { get; set; } = null!;

    }
}
