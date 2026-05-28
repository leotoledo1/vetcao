using VetClinic.DTOs.Requests;
using VetClinic.Entities;
using VetClinic.Interfaces.IRepositories;

namespace VetClinic.UseCases
{
    // Caso de uso responsavel por validar e agendar consultas.
    /// ========================================
    /// USE CASE: AgendarConsultaUseCase
    /// RESPONSABILIDADE: Agendar uma consulta veterinária
    /// 
    /// Um Use Case representa uma ação específica do sistema
    /// "Agendar Consulta" é um Use Case que encapsula toda a lógica
    /// 
    /// VALIDAÇÕES IMPORTANTES:
    /// 1. O Pet deve existir
    /// 2. O Veterinário deve existir
    /// 3. O Veterinário não pode ter consulta no mesmo horário (+/- 30 minutos)
    /// 4. A consulta é criada com status "Agendada"
    /// ========================================
    public class AgendarConsultaUseCase
    {
        // Dependências: repositórios para acessar o banco
        private readonly IConsultaRepository _consultaRepository;
        private readonly IPetRepository _petRepository;
        private readonly IVeterinarioRepository _veterinarioRepository;

        public AgendarConsultaUseCase(
            IConsultaRepository consultaRepository,
            IPetRepository petRepository,
            IVeterinarioRepository veterinarioRepository)
        {
            _consultaRepository = consultaRepository;
            _petRepository = petRepository;
            _veterinarioRepository = veterinarioRepository;
        }

        /// <summary>
        /// MÉTODO: Run (executar)
        /// RESPONSABILIDADE: Executar todo o processo de agendamento
        /// 
        /// FLUXO COMPLETO:
        /// 1. Validar que o Pet existe
        /// 2. Validar que o Veterinário existe
        /// 3. Validar que não há conflito de horário
        /// 4. Criar a consulta
        /// 5. Validar os dados da consulta
        /// 6. Salvar no banco
        /// </summary>
        public void Run(AgendarConsultaRequest request)
        {
            try
            {
                // VALIDAÇÃO 1: Verificar se o Pet existe
                Pet? pet = _petRepository.FindById(request.PetId);
                if (pet == null)
                {
                    throw new Exception("Pet não encontrado");
                }

                // VALIDAÇÃO 2: Verificar se o Veterinário existe
                Veterinario? vet = _veterinarioRepository.FindById(request.VeterinarioId);
                if (vet == null)
                {
                    throw new Exception("Veterinário não encontrado");
                }

                // VALIDAÇÃO 3: Verificar conflito de horário
                // Este é o método mais crítico em termos de performance!
                // Verifica se o veterinário já tem consulta nesse horário (+/- 30 min)
                bool temConflito = _consultaRepository.TemConflitoDeHorario(
                    request.VeterinarioId, request.DataHora
                );

                if (temConflito)
                {
                    throw new Exception("Veterinário já possui consulta neste horário");
                }

                // PASSO 4: Criar a nova Consulta
                Consulta consulta = new Consulta();
                consulta.PetId = request.PetId;
                consulta.VeterinarioId = request.VeterinarioId;
                consulta.DataHora = request.DataHora;
                consulta.Motivo = request.Motivo;
                consulta.Status = "Agendada";           // Status inicial
                consulta.CreatedAt = DateTime.Now;      // Data/hora de criação

                // PASSO 5: Validar os dados da consulta (se houver validações)
                consulta.Validar();

                // PASSO 6: Salvar no banco de dados
                _consultaRepository.Create(consulta);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
