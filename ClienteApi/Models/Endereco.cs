namespace ClienteApi.Models
{
    public class Endereco
    {
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Cep { get; set; }

        public bool EhValido()
        {
            return !string.IsNullOrWhiteSpace(Rua)
                && !string.IsNullOrWhiteSpace(Numero)
                && !string.IsNullOrWhiteSpace(Cidade)
                && !string.IsNullOrWhiteSpace(Estado)
                && !string.IsNullOrWhiteSpace(Cep);
        }
    }
}
