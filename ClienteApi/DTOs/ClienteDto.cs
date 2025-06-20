using System.ComponentModel.DataAnnotations;

namespace ClienteApi.DTOs
{
    public class ClienteCreateDto
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Telefone { get; set; }

        [Required]
        public EnderecoDto Endereco { get; set; }
    }

    public class ClienteDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public EnderecoDto Endereco { get; set; }
    }
}
