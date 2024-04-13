namespace FantaxyWebApplication.Models
{
    public class CreatePlanetModel
    {
        public CreatePlanetModel() { }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public bool Status { get; set; }

        public byte[] Avatar { get; set; }

        public byte[] Main { get; set; }

        public byte[] Profile { get; set; }
    }
}
