using VetClinic.DTOs;
using VetClinic.Entities;
using VetClinic.Interfaces.IRepositories;

namespace VetClinic.UseCases
{
    // Caso de uso responsavel por criar um pet novo.
    public class CadastrarPetUseCase
    {
        private readonly IPetRepository _petRepository;
        private readonly IDonoRepository _donoRepository;

        public CadastrarPetUseCase(IPetRepository petRepository, IDonoRepository donoRepository)
        {
            _petRepository = petRepository;
            _donoRepository = donoRepository;
        }

        public void Run(CreatePetDTO dto)
        {
            try
            {
                Dono? dono = _donoRepository.FindById(dto.DonoId);
                if (dono == null)
                {
                    throw new Exception("Dono não encontrado");
                }

                Pet pet = new Pet();
                pet.Nome = dto.Nome;
                pet.Especie = dto.Especie;
                pet.Raca = dto.Raca;
                pet.DataNascimento = dto.DataNascimento;
                pet.DonoId = dto.DonoId;
                pet.CreatedAt = DateTime.Now;

                pet.Validar();
                _petRepository.Create(pet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
