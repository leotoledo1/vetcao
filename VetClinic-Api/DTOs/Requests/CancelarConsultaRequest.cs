namespace VetClinic.DTOs.Requests
{
    // Requisicao pequena usada para cancelar uma consulta.
    public class CancelarConsultaRequest
    {
        public Guid ConsultaId { get; set; }
    }
}
