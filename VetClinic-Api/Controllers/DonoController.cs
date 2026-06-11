using Microsoft.AspNetCore.Mvc;
using VetClinic.DTOs;
using VetClinic.Interfaces.IServices;
using VetClinic.UseCases;

namespace VetClinic.Controllers
{
                                                              
                                                
                                  
                                                              
        
                                                
                                                           
        
                     
                                     
                                      
                                                 
                                  
                                                
    [ApiController]
    [Route("[controller]")]                      
    public class DonoController : ControllerBase
    {
        private readonly IDonoService _service;
        private readonly CadastrarDonoUseCase _cadastrarDonoUseCase;

        public DonoController(IDonoService service, CadastrarDonoUseCase cadastrarDonoUseCase)
        {
                                                                          
                                                  
                                                     
            _service = service;
            _cadastrarDonoUseCase = cadastrarDonoUseCase;
        }

                     
                               
                                                           
                                                               
            
                  
                                                   
                                    
                                                              
                                                       
                      
        [HttpGet("ListarDonos")]
        public IActionResult ListarDonos()
        {
            try
            {
                                                                             
                var list = _service.ListarDonos();
                
                                                      
                return Ok(list);
            }
            catch (Exception ex)
            {
                                                                                     
                return BadRequest(ex.Message);
            }
        }

                     
                           
                                                                                               
                                                               
            
                  
                                                           
                                                 
                                                        
                                                      
                      
        [HttpGet("GetById")]
        public IActionResult GetById([FromQuery] Guid id)
        {
            try
            {
                                                      
                var dono = _service.GetById(id);
                
                                                       
                if (dono == null)
                {
                    return NotFound();
                }

                                                            
                return Ok(dono);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

                     
                                 
                                                              
                                                                                                                   
                                                                  
            
                  
                                                         
                                                                
                                                       
                                                                    
                                                                   
                      
        [HttpPost("CadastrarDono")]
        public IActionResult CadastrarDono([FromBody] CreateDonoDTO dto)
        {
            try
            {
                                                                          
                                                                                                    
                _cadastrarDonoUseCase.Run(dto);
                
                                                               
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                                                                                         
                return BadRequest(ex.Message);
            }
        }
    }
}
