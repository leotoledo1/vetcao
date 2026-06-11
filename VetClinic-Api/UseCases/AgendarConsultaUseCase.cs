using VetClinic.DTOs.Requests;
using VetClinic.Entities;
using VetClinic.Interfaces.IRepositories;

namespace VetClinic.UseCases
{
                                                               
                                                
                                        
                                                          
        
                                                             
                                                                    
        
                               
                             
                                     
                                                                                
                                                    
                                                
    public class AgendarConsultaUseCase
    {
                                                          
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

                     
                                  
                                                                     
            
                           
                                       
                                               
                                                     
                               
                                           
                              
                      
        public void Run(AgendarConsultaRequest request)
        {
            try
            {
                                                         
                Pet? pet = _petRepository.FindById(request.PetId);
                if (pet == null)
                {
                    throw new Exception("Pet não encontrado");
                }

                                                                 
                Veterinario? vet = _veterinarioRepository.FindById(request.VeterinarioId);
                if (vet == null)
                {
                    throw new Exception("Veterinário não encontrado");
                }

                                                             
                                                                         
                                                                                       
                bool temConflito = _consultaRepository.TemConflitoDeHorario(
                    request.VeterinarioId, request.DataHora
                );

                if (temConflito)
                {
                    throw new Exception("Veterinário já possui consulta neste horário");
                }

                                                 
                Consulta consulta = new Consulta();
                consulta.PetId = request.PetId;
                consulta.VeterinarioId = request.VeterinarioId;
                consulta.DataHora = request.DataHora;
                consulta.Motivo = request.Motivo;
                consulta.Status = "Agendada";                            
                consulta.CreatedAt = DateTime.Now;                             

                                                                               
                consulta.Validar();

                                                    
                _consultaRepository.Create(consulta);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
