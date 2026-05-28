using Microsoft.AspNetCore.Mvc;
using VetClinic.DTOs;
using VetClinic.Interfaces.IServices;
using VetClinic.UseCases;

namespace VetClinic.Controllers
{
    // Recebe chamadas do front para listar e cadastrar donos.
    /// ========================================
    /// CONTROLLER: DonoController
    /// RESPONSABILIDADE: Receber requisições HTTP sobre DONOS
    /// 
    /// Uma requisição HTTP chega aqui primeiro!
    /// Exemplo: GET http://localhost:5000/dono/listardonos
    /// 
    /// O Controller:
    /// 1. Recebe os dados do cliente
    /// 2. Valida os dados basicamente
    /// 3. Chama o Service/UseCase para processar
    /// 4. Retorna a resposta HTTP
    /// ========================================
    [ApiController]
    [Route("[controller]")]  // Rota base = /dono
    public class DonoController : ControllerBase
    {
        private readonly IDonoService _service;
        private readonly CadastrarDonoUseCase _cadastrarDonoUseCase;

        public DonoController(IDonoService service, CadastrarDonoUseCase cadastrarDonoUseCase)
        {
            // Injeção de Dependência: O .NET nos fornece essas instâncias
            // Service: contém a lógica de negócio
            // UseCase: encapsula uma ação específica
            _service = service;
            _cadastrarDonoUseCase = cadastrarDonoUseCase;
        }

        /// <summary>
        /// MÉTODO: ListarDonos
        /// URL: GET http://localhost:5000/dono/listardonos
        /// RESPONSABILIDADE: Buscar TODOS os donos cadastrados
        /// 
        /// FLUXO:
        /// 1. Frontend envia GET /dono/listardonos
        /// 2. Este método é chamado
        /// 3. Chama _service.ListarDonos() que acessa o banco
        /// 4. Retorna lista de donos com status 200 OK
        /// </summary>
        [HttpGet("ListarDonos")]
        public IActionResult ListarDonos()
        {
            try
            {
                // Chama o Service que busca todos os donos do banco de dados
                var list = _service.ListarDonos();
                
                // Retorna 200 OK com a lista de donos
                return Ok(list);
            }
            catch (Exception ex)
            {
                // Se algo der errado, retorna 400 Bad Request com a mensagem de erro
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// MÉTODO: GetById
        /// URL: GET http://localhost:5000/dono/getbyid?id=123e4567-e89b-12d3-a456-426614174000
        /// RESPONSABILIDADE: Buscar UM dono específico pelo ID
        /// 
        /// FLUXO:
        /// 1. Frontend envia GET com o ID do dono que quer
        /// 2. O Service busca esse dono no banco
        /// 3. Se encontrar: retorna 200 OK com os dados
        /// 4. Se não encontrar: retorna 404 Not Found
        /// </summary>
        [HttpGet("GetById")]
        public IActionResult GetById([FromQuery] Guid id)
        {
            try
            {
                // Busca o dono com esse ID específico
                var dono = _service.GetById(id);
                
                // Se não encontrou o dono, retorna 404
                if (dono == null)
                {
                    return NotFound();
                }

                // Se encontrou, retorna 200 OK com os dados
                return Ok(dono);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// MÉTODO: CadastrarDono
        /// URL: POST http://localhost:5000/dono/cadastrardono
        /// BODY: { "Nome": "João", "Cpf": "123.456.789-00", "Telefone": "11999999999", "Email": "joao@email.com" }
        /// RESPONSABILIDADE: Criar um novo dono no banco de dados
        /// 
        /// FLUXO:
        /// 1. Frontend envia POST com dados do novo dono
        /// 2. Extrai os dados do corpo da requisição (FromBody)
        /// 3. Chama o UseCase que valida e cria o dono
        /// 4. Se bem-sucedido: retorna 201 Created (recurso criado)
        /// 5. Se erro (ex: CPF duplicado): retorna 400 Bad Request
        /// </summary>
        [HttpPost("CadastrarDono")]
        public IActionResult CadastrarDono([FromBody] CreateDonoDTO dto)
        {
            try
            {
                // Chama o UseCase que encapsula toda a lógica de cadastro
                // O UseCase faz: validação de CPF, verifica duplicatas, cria o dono, salva no banco
                _cadastrarDonoUseCase.Run(dto);
                
                // 201 Created = recurso foi criado com sucesso
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                // Se algo deu errado (CPF já existe, dados inválidos, etc), retorna erro
                return BadRequest(ex.Message);
            }
        }
    }
}
