using System.ComponentModel.DataAnnotations;

namespace ClienteApi.Models
{
    public class Cliente
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Telefone { get; set; }

        [Required]
        public Endereco Endereco { get; set; }

        public bool EhValido()
        {
            return !string.IsNullOrWhiteSpace(Nome)
                && !string.IsNullOrWhiteSpace(Email)
                && Endereco?.EhValido() == true;
        }
    }
}
