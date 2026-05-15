namespace VetClinic.Entities
{
    public class Consulta : EntityBase
    {
        public Guid PetId { get; set; }
        public Pet Pet { get; set; } = null!;
        public Guid VeterinarioId { get; set; }
        public Veterinario Veterinario { get; set; } = null!;
        public DateTime DataHora { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public Consulta() { }

        public void Validar()
        {
            if (DataHora < DateTime.Now)
            {
                throw new Exception("A data da consulta não pode ser no passado");
            }

            if (string.IsNullOrEmpty(Motivo))
            {
                throw new Exception("Motivo da consulta é obrigatório");
            }
        }

        public void Cancelar()
        {
            if (Status != "Agendada")
            {
                throw new Exception("Apenas consultas com status Agendada podem ser canceladas");
            }

            Status = "Cancelada";
        }
    }
}
