using System.ComponentModel.DataAnnotations;

namespace ClienteApi.DTOs
{
    public class EnderecoDto
    {
        [Required]
        public string Rua { get; set; }

        [Required]
        public string Numero { get; set; }

        [Required]
        public string Cidade { get; set; }

        [Required]
        public string Estado { get; set; }

        [Required]
        public string Cep { get; set; }
    }
}
