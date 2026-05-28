namespace VetClinic.DTOs
{
    // DTO de apoio para representar dados de consulta.
    public class CreateConsultaDTO
    {
        public Guid PetId { get; set; }
        public Guid VeterinarioId { get; set; }
        public DateTime DataHora { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }
}
