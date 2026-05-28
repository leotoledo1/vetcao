using VetClinic.Interfaces.IRepositories;

namespace VetClinic.UseCases
{
    // Caso de uso responsavel por cancelar uma consulta agendada.
    public class CancelarConsultaUseCase
    {
        private readonly IConsultaRepository _consultaRepository;

        public CancelarConsultaUseCase(IConsultaRepository consultaRepository)
        {
            _consultaRepository = consultaRepository;
        }

        public void Run(Guid consultaId)
        {
            try
            {
                var consulta = _consultaRepository.FindById(consultaId);
                if (consulta == null)
                {
                    throw new Exception("Consulta não encontrada");
                }

                consulta.Cancelar();
                _consultaRepository.Update(consulta);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
