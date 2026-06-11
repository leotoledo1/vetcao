using VetClinic.Entities;

namespace VetClinic.Interfaces.IServices
{
                                        
    public interface IConsultaService
    {
        public List<Consulta> ListarConsultas();
        public Consulta? GetById(Guid id);
    }
}
