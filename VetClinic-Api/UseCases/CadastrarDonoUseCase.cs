using VetClinic.DTOs;
using VetClinic.Entities;
using VetClinic.Interfaces.IRepositories;
using VetClinic.ValueObjects;

namespace VetClinic.UseCases
{
    // Caso de uso responsavel por criar um dono novo.
    public class CadastrarDonoUseCase
    {
        private readonly IDonoRepository _donoRepository;

        public CadastrarDonoUseCase(IDonoRepository donoRepository)
        {
            _donoRepository = donoRepository;
        }

        public void Run(CreateDonoDTO dto)
        {
            try
            {
                bool cpfExiste = _donoRepository.CpfJaExiste(
                    new string(dto.Cpf.Where(char.IsDigit).ToArray())
                );

                if (cpfExiste)
                {
                    throw new Exception("CPF já cadastrado no sistema");
                }

                Dono dono = new Dono();
                dono.Nome = dto.Nome;
                dono.Cpf = new Cpf(dto.Cpf);
                dono.Telefone = dto.Telefone;
                dono.Email = dto.Email;
                dono.CreatedAt = DateTime.Now;

                _donoRepository.Create(dono);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
