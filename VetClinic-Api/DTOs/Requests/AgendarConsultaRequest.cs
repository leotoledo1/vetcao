namespace VetClinic.DTOs.Requests
{
    // Requisicao enviada pelo front para agendar uma consulta.
    public class AgendarConsultaRequest
    {
        public Guid PetId { get; set; }
        public Guid VeterinarioId { get; set; }
        public DateTime DataHora { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }
}
