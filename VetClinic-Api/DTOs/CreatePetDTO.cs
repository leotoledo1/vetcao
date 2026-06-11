namespace VetClinic.DTOs
{
                                                                 
    public class CreatePetDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string Especie { get; set; } = string.Empty;
        public string Raca { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public Guid DonoId { get; set; }
    }
}
