using VetClinic.Data;
using VetClinic.Entities;
using VetClinic.Interfaces.IRepositories;

namespace VetClinic.Repositories
{
    // Acesso ao banco para operacoes com donos.
    /// ========================================
    /// REPOSITORY: DonoRepository
    /// RESPONSABILIDADE: Acesso ao banco de dados (CRUD)
    /// 
    /// O Repository isola a lógica de banco de dados
    /// Se precisar mudar de SQLite para SQL Server, só muda aqui!
    /// ========================================
    public class DonoRepository : IDonoRepository
    {
        public readonly Context _database;

        // Injeção de dependência: recebe a conexão com o banco de dados
        public DonoRepository(Context context)
        {
            _database = context;
        }

        /// <summary>
        /// MÉTODO: Create
        /// RESPONSABILIDADE: Inserir um novo dono no banco
        /// BANCO: INSERT INTO Donos (...) VALUES (...)
        /// </summary>
        public void Create(Dono dono)
        {
            // Adiciona o dono na tabela Donos
            _database.Donos.Add(dono);
            
            // SaveChanges() executa o SQL no banco de dados
            // Sem isso, o dono não é salvo de verdade
            _database.SaveChanges();
        }

        /// <summary>
        /// MÉTODO: Delete
        /// RESPONSABILIDADE: Deletar um dono do banco (delete real, não soft delete)
        /// BANCO: DELETE FROM Donos WHERE ...
        /// </summary>
        public void Delete(Dono dono)
        {
            _database.Donos.Remove(dono);
            _database.SaveChanges();
        }

        /// <summary>
        /// MÉTODO: GetAll
        /// RESPONSABILIDADE: Buscar TODOS os donos (que não foram deletados)
        /// BANCO: SELECT * FROM Donos WHERE RemovedAt IS NULL ORDER BY Nome
        /// 
        /// IMPORTANTE:
        /// - Filtra RemovedAt == null (não inclui donos deletados)
        /// - Ordena por Nome (A-Z)
        /// </summary>
        public List<Dono> GetAll()
        {
            return _database.Donos
                // Filtra: só incluir donos que NÃO foram deletados
                .Where(d => d.RemovedAt == null)
                
                // Ordena por nome (crescente: A-Z)
                .OrderBy(d => d.Nome)
                
                // Executa no banco e traz para memória como List
                .ToList();
        }

        /// <summary>
        /// MÉTODO: FindById
        /// RESPONSABILIDADE: Buscar UM dono específico pelo ID
        /// BANCO: SELECT * FROM Donos WHERE Id = ? AND RemovedAt IS NULL
        /// 
        /// Retorna: O dono se encontrou, null se não encontrou
        /// </summary>
        public Dono? FindById(Guid id)
        {
            return _database.Donos
                // Filtra por ID e que não foi deletado
                .Where(d => d.Id == id && d.RemovedAt == null)
                
                // FirstOrDefault retorna o primeiro ou null se não existe
                .FirstOrDefault();
        }

        /// <summary>
        /// MÉTODO: CpfJaExiste
        /// RESPONSABILIDADE: Verificar se um CPF já está cadastrado
        /// BANCO: SELECT COUNT(*) FROM Donos WHERE Cpf.Value = ? AND RemovedAt IS NULL
        /// 
        /// Retorna: true se já existe, false se não existe
        /// 
        /// ⚠️ IMPORTANTE: Sem índice no banco, isso é O(n) = lento!
        /// Com índice, fica O(log n) = rápido
        /// </summary>
        public bool CpfJaExiste(string cpf)
        {
            // Any() retorna true se encontrou, false se não encontrou
            return _database.Donos
                .Any(d => d.Cpf.Value == cpf && d.RemovedAt == null);
        }

        /// <summary>
        /// MÉTODO: Update
        /// RESPONSABILIDADE: Atualizar um dono existente no banco
        /// BANCO: UPDATE Donos SET ... WHERE Id = ?
        /// </summary>
        public void Update(Dono dono)
        {
            _database.Donos.Update(dono);
            _database.SaveChanges();
        }
    }
}
