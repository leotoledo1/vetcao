namespace VetClinic.Entities
{
                                                           
    public class Veterinario : EntityBase
    {
        public string Nome { get; set; } = string.Empty;
        public string Crmv { get; set; } = string.Empty;
        public string Especialidade { get; set; } = string.Empty;

        public Veterinario() { }

        public void Validar()
        {
            if (string.IsNullOrEmpty(Nome))
            {
                throw new Exception("Nome do veterinário é obrigatório");
            }

            if (string.IsNullOrEmpty(Crmv))
            {
                throw new Exception("CRMV é obrigatório");
            }
        }
    }
}
