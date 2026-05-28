using Microsoft.AspNetCore.Mvc;
using VetClinic.DTOs.Requests;
using VetClinic.Interfaces.IRepositories;
using VetClinic.Interfaces.IServices;
using VetClinic.UseCases;

namespace VetClinic.Controllers
{
    // Controla as rotas de consulta e entrega os dados para o front.
    [ApiController]
    [Route("[controller]")]
    public class ConsultaController : ControllerBase
    {
        private readonly AgendarConsultaUseCase _agendarUseCase;
        private readonly CancelarConsultaUseCase _cancelarUseCase;
        private readonly IConsultaService _service;
        private readonly IVeterinarioRepository _veterinarioRepository;

        public ConsultaController(
            AgendarConsultaUseCase agendarUseCase,
            CancelarConsultaUseCase cancelarUseCase,
            IConsultaService service,
            IVeterinarioRepository veterinarioRepository)
        {
            _agendarUseCase = agendarUseCase;
            _cancelarUseCase = cancelarUseCase;
            _service = service;
            _veterinarioRepository = veterinarioRepository;
        }

        [HttpGet("ListarConsultas")]
        public IActionResult ListarConsultas()
        {
            try
            {
                var list = _service.ListarConsultas();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ListarVeterinarios")]
        public IActionResult ListarVeterinarios()
        {
            try
            {
                var list = _veterinarioRepository.GetAll();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Agendar")]
        public IActionResult Agendar([FromBody] AgendarConsultaRequest request)
        {
            try
            {
                _agendarUseCase.Run(request);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("Cancelar")]
        public IActionResult Cancelar([FromQuery] Guid id)
        {
            try
            {
                _cancelarUseCase.Run(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
