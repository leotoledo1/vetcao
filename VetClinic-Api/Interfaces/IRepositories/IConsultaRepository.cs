using VetClinic.Entities;

namespace VetClinic.Interfaces.IRepositories
{
    // Contrato do repositorio de consultas.
    public interface IConsultaRepository
    {
        public List<Consulta> GetAll();
        public Consulta? FindById(Guid id);
        public void Create(Consulta consulta);
        public void Update(Consulta consulta);
        public bool TemConflitoDeHorario(Guid veterinarioId, DateTime dataHora);
    }
}
