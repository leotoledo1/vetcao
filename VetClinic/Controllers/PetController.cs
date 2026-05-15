using Microsoft.AspNetCore.Mvc;
using VetClinic.DTOs;
using VetClinic.Interfaces.IRepositories;
using VetClinic.UseCases;

namespace VetClinic.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetController : ControllerBase
    {
        private readonly CadastrarPetUseCase _cadastrarPetUseCase;
        private readonly ExcluirPetUseCase _excluirPetUseCase;
        private readonly IPetRepository _petRepository;

        public PetController(
            CadastrarPetUseCase cadastrarPetUseCase,
            ExcluirPetUseCase excluirPetUseCase,
            IPetRepository petRepository)
        {
            _cadastrarPetUseCase = cadastrarPetUseCase;
            _excluirPetUseCase = excluirPetUseCase;
            _petRepository = petRepository;
        }

        [HttpGet("ListarPets")]
        public IActionResult ListarPets()
        {
            try
            {
                var list = _petRepository.GetAll();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CadastrarPet")]
        public IActionResult CadastrarPet([FromBody] CreatePetDTO dto)
        {
            try
            {
                _cadastrarPetUseCase.Run(dto);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("ExcluirPet")]
        public IActionResult ExcluirPet([FromQuery] Guid id)
        {
            try
            {
                _excluirPetUseCase.Run(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
