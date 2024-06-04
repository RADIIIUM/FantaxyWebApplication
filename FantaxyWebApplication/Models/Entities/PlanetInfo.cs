using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FantaxyWebApplication.Models.Entities
{
    public partial class PlanetInfo
    {
        public int IdPlanet { get; set; }
        [Required(ErrorMessage = "Пустое имя")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Слишком короткий/длинный никнейм")]
        public string PlanetName { get; set; } = null!;
        public string? PlanetDescription { get; set; }
        public string Avatar { get; set; } = null!;
        public string ProfileBackground { get; set; } = null!;
        public string MainBackground { get; set; } = null!;

        public virtual Planet IdPlanetNavigation { get; set; } = null!;
    }
}
