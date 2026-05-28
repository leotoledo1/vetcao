using VetClinic.Interfaces.IRepositories;

namespace VetClinic.UseCases
{
    // Caso de uso responsavel por remover um pet com seguranca.
    public class ExcluirPetUseCase
    {
        private readonly IPetRepository _petRepository;
        private readonly IConsultaRepository _consultaRepository;

        public ExcluirPetUseCase(IPetRepository petRepository, IConsultaRepository consultaRepository)
        {
            _petRepository = petRepository;
            _consultaRepository = consultaRepository;
        }

        public void Run(Guid petId)
        {
            try
            {
                var pet = _petRepository.FindById(petId);
                if (pet == null)
                {
                    throw new Exception("Pet não encontrado");
                }

                bool temConsultaFutura = _petRepository.TemConsultaFutura(petId);

                if (temConsultaFutura)
                {
                    throw new Exception("Não é possível excluir pet com consultas futuras agendadas");
                }

                pet.RemovedAt = DateTime.Now;
                _petRepository.Update(pet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
