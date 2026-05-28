namespace VetClinic.DTOs
{
    // DTO usado pelo front para enviar dados de cadastro de dono.
    public class CreateDonoDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
