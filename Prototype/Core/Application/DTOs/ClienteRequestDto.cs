
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Generators.Abstractions; // ✅ este es el correcto

namespace Application.DTOs
{
    [GenerateMapper(typeof(Domain.Entities.Cliente))]
    public class ClienteRequestDto
    {
        //Identity
        [JsonPropertyName("Id")]
        [Required]
        public int Id { get; set; }

        [JsonPropertyName("Nombre")]
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [JsonPropertyName("Apellido")]
        [StringLength(50)]
        public string Apellido { get; set; }

        [JsonPropertyName("Email")]
        [Required]
        [StringLength(50)]
        public string Email { get; set; }

    }
}