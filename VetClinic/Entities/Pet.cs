namespace VetClinic.Entities
{
    public class Pet : EntityBase
    {
        public string Nome { get; set; } = string.Empty;
        public string Especie { get; set; } = string.Empty;
        public string Raca { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public Guid DonoId { get; set; }
        public Dono Dono { get; set; } = null!;

        public Pet() { }

        public void Validar()
        {
            if (string.IsNullOrEmpty(Nome))
            {
                throw new Exception("Nome do pet é obrigatório");
            }

            if (DataNascimento > DateTime.Now)
            {
                throw new Exception("Data de nascimento não pode ser futura");
            }
        }
    }
}
