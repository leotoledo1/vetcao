using VetClinic.Entities;

namespace VetClinic.Interfaces.IServices
{
    // Contrato do service de consultas.
    public interface IConsultaService
    {
        public List<Consulta> ListarConsultas();
        public Consulta? GetById(Guid id);
    }
}
