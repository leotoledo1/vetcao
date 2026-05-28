using Microsoft.EntityFrameworkCore;
using VetClinic.Data;
using VetClinic.Entities;
using VetClinic.Interfaces.IRepositories;

namespace VetClinic.Repositories
{
    // Acesso ao banco para consultas com include de pet e veterinario.
    /// ========================================
    /// REPOSITORY: ConsultaRepository
    /// RESPONSABILIDADE: Acesso ao banco de dados de CONSULTAS
    /// 
    /// Métodos principais:
    /// - Create: adicionar nova consulta
    /// - GetAll: listar todas as consultas com dados do Pet e Vet
    /// - FindById: buscar uma consulta específica
    /// - TemConflitoDeHorario: verificar se veterinário está livre
    /// - Update: atualizar consulta existente
    /// ========================================
    public class ConsultaRepository : IConsultaRepository
    {
        public readonly Context _database;

        public ConsultaRepository(Context context)
        {
            _database = context;
        }

        /// <summary>
        /// MÉTODO: Create
        /// RESPONSABILIDADE: Inserir uma nova consulta no banco
        /// </summary>
        public void Create(Consulta consulta)
        {
            _database.Consultas.Add(consulta);
            _database.SaveChanges();
        }

        /// <summary>
        /// MÉTODO: GetAll
        /// RESPONSABILIDADE: Buscar TODAS as consultas com dados completos
        /// 
        /// IMPORTANTE:
        /// Include(c => c.Pet) = traz os dados do Pet associado
        /// Include(c => c.Veterinario) = traz os dados do Veterinário
        /// OrderBy(c => c.DataHora) = ordena por data/hora (mais próximas primeiro)
        /// 
        /// Sem Include() você teria que fazer queries separadas para Pet e Vet
        /// Com Include() você busca tudo de uma vez (mais eficiente)
        /// </summary>
        public List<Consulta> GetAll()
        {
            return _database.Consultas
                // Include: traz dados relacionados do Pet
                .Include(c => c.Pet)
                
                // Include: traz dados relacionados do Veterinário
                .Include(c => c.Veterinario)
                
                // OrderBy: ordena por data/hora crescente
                .OrderBy(c => c.DataHora)
                
                // ToList: executa no banco e traz os dados para a memória
                .ToList();
        }

        /// <summary>
        /// MÉTODO: FindById
        /// RESPONSABILIDADE: Buscar UMA consulta específica pelo ID
        /// 
        /// Retorna a consulta COM os dados do Pet e Veterinário
        /// </summary>
        public Consulta? FindById(Guid id)
        {
            return _database.Consultas
                .Include(c => c.Pet)
                .Include(c => c.Veterinario)
                .Where(c => c.Id == id)
                .FirstOrDefault();
        }

        /// <summary>
        /// MÉTODO: TemConflitoDeHorario (CRÍTICO!)
        /// RESPONSABILIDADE: Verificar se um veterinário está livre em um horário
        /// 
        /// LÓGICA:
        /// - Um veterinário não pode ter 2 consultas muito próximas
        /// - Cria uma "janela de tempo" de 30 minutos antes e depois (total 1 hora)
        /// - Se houver qualquer consulta "Agendada" nessa janela, há conflito
        /// 
        /// EXEMPLO:
        /// Se quero agendar às 14:00
        /// Janela = 13:30 até 14:30
        /// Se há consulta às 13:50 ou 14:15, há conflito!
        /// 
        /// ⚠️ PROBLEMA DE PERFORMANCE:
        /// Sem índice no banco, este método varre TODAS as consultas = O(n) LENTO!
        /// Com índice em (VeterinarioId, Status, DataHora), fica O(log n) RÁPIDO!
        /// </summary>
        public bool TemConflitoDeHorario(Guid veterinarioId, DateTime dataHora)
        {
            // Cria uma janela de 30 minutos antes
            DateTime inicioJanela = dataHora.AddMinutes(-30);
            
            // Cria uma janela de 30 minutos depois
            DateTime fimJanela = dataHora.AddMinutes(30);

            // Verifica se há alguma consulta nessa janela de tempo
            return _database.Consultas
                
                // Filtra por veterinário específico
                .Any(c => c.VeterinarioId == veterinarioId
                
                       // Apenas consultas que ainda estão agendadas (não canceladas/realizadas)
                       && c.Status == "Agendada"
                       
                       // Data/hora está entre o início e fim da janela
                       && c.DataHora > inicioJanela
                       && c.DataHora < fimJanela);
        }

        /// <summary>
        /// MÉTODO: Update
        /// RESPONSABILIDADE: Atualizar uma consulta existente
        /// 
        /// Exemplo: mudar status de "Agendada" para "Realizada"
        /// ou "Cancelada"
        /// </summary>
        public void Update(Consulta consulta)
        {
            _database.Consultas.Update(consulta);
            _database.SaveChanges();
        }
    }
}
