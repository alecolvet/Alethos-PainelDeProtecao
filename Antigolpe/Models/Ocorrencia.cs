namespace Antigolpe.Models
{
    public class Ocorrencia
    {
        public int Id { get; set; }

        public string Mensagem { get; set; } = string.Empty;

        public string ClassificacaoIA { get; set; } = string.Empty;

        public string NivelRisco { get; set; } = string.Empty;

        public string MotivoAnalise { get; set; } = string.Empty;

        public string Status { get; set; } = "Pendente";

        public DateTime DataRegistro { get; set; } = DateTime.Now;
    }
}
