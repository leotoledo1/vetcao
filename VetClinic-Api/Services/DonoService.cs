using VetClinic.DTOs;
using VetClinic.Entities;
using VetClinic.Interfaces.IRepositories;
using VetClinic.Interfaces.IServices;
using VetClinic.ValueObjects;

namespace VetClinic.Services
{
    // Regra de negocio para donos: valida CPF e protege contra duplicidade.
    /// ========================================
    /// SERVICE: DonoService
    /// RESPONSABILIDADE: Lógica de negócio relacionada a DONOS
    /// 
    /// O Service é chamado pelo Controller
    /// Aqui acontecem validações, regras de negócio, etc
    /// ========================================
    public class DonoService : IDonoService
    {
        private readonly IDonoRepository _repository;

        // Injeção de Dependência: recebe o Repository para acessar o banco
        public DonoService(IDonoRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// MÉTODO: CriarDono
        /// RESPONSABILIDADE: Validar e criar um novo dono
        /// 
        /// VALIDAÇÕES QUE FAZ:
        /// 1. Remove caracteres especiais do CPF (vai deixar só números)
        /// 2. Verifica se esse CPF já existe no banco
        /// 3. Se já existe, lança exceção (erro)
        /// 4. Se não existe, cria o dono e salva no banco
        /// </summary>
        public void CriarDono(CreateDonoDTO dto)
        {
            try
            {
                // Remove caracteres especiais do CPF (pontos, hífens)
                // "123.456.789-00" vira "12345678900"
                var cpf = new string(dto.Cpf.Where(char.IsDigit).ToArray());
                
                // Verifica se esse CPF já está cadastrado
                bool cpfExiste = _repository.CpfJaExiste(cpf);

                if (cpfExiste)
                {
                    // Se já existe, lança erro
                    throw new Exception("CPF já cadastrado no sistema");
                }

                // Cria uma nova entidade Dono
                Dono dono = new Dono();
                dono.Nome = dto.Nome;
                dono.Cpf = new Cpf(dto.Cpf);        // ValueObject que valida o CPF
                dono.Telefone = dto.Telefone;
                dono.Email = dto.Email;
                dono.CreatedAt = DateTime.Now;      // Registra quando foi criado

                // Salva no banco através do Repository
                _repository.Create(dono);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// MÉTODO: DeletarDono
        /// RESPONSABILIDADE: Deletar (marcar como removido) um dono
        /// 
        /// IMPORTANTE: Não deleta de verdade do banco!
        /// Apenas marca com RemovedAt = agora
        /// Isso é "soft delete" - protege os dados históricos
        /// </summary>
        public void DeletarDono(Guid id)
        {
            try
            {
                // Busca o dono pelo ID
                Dono? dono = _repository.FindById(id);
                if (dono == null)
                {
                    throw new Exception("Dono não encontrado");
                }

                // Marca como removido (soft delete)
                // A linha não é deletada do banco, só marcada como deletada
                dono.RemovedAt = DateTime.Now;
                
                // Atualiza no banco
                _repository.Update(dono);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// MÉTODO: GetById
        /// RESPONSABILIDADE: Buscar um dono específico pelo ID
        /// 
        /// FLUXO:
        /// 1. Chama o Repository para buscar no banco
        /// 2. Se encontrou, retorna o dono
        /// 3. Se não encontrou, retorna null
        /// </summary>
        public Dono? GetById(Guid id)
        {
            try
            {
                // Busca no banco
                Dono? dono = _repository.FindById(id);
                if (dono == null)
                {
                    return null;
                }

                return dono;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Dono> ListarDonos()
        {
            try
            {
                List<Dono> list = _repository.GetAll();
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
