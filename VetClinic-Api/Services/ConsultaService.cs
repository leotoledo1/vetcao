using VetClinic.Entities;
using VetClinic.Interfaces.IRepositories;
using VetClinic.Interfaces.IServices;

namespace VetClinic.Services
{
    // Camada de leitura das consultas usadas pelo controller.
    public class ConsultaService : IConsultaService
    {
        private readonly IConsultaRepository _repository;

        public ConsultaService(IConsultaRepository repository)
        {
            _repository = repository;
        }

        public Consulta? GetById(Guid id)
        {
            try
            {
                return _repository.FindById(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Consulta> ListarConsultas()
        {
            try
            {
                return _repository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
