using VetClinic.DTOs;
using VetClinic.Entities;
using VetClinic.Interfaces.IRepositories;
using VetClinic.Interfaces.IServices;
using VetClinic.ValueObjects;

namespace VetClinic.Services
{
                                                                            
                                                
                            
                                                               
        
                                           
                                                         
                                                
    public class DonoService : IDonoService
    {
        private readonly IDonoRepository _repository;

                                                                           
        public DonoService(IDonoRepository repository)
        {
            _repository = repository;
        }

                     
                             
                                                          
            
                               
                                                                         
                                                      
                                                 
                                                          
                      
        public void CriarDono(CreateDonoDTO dto)
        {
            try
            {
                                                                      
                                                      
                var cpf = new string(dto.Cpf.Where(char.IsDigit).ToArray());
                
                                                          
                bool cpfExiste = _repository.CpfJaExiste(cpf);

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

                                                       
                _repository.Create(dono);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

                     
                               
                                                                    
            
                                                       
                                              
                                                              
                      
        public void DeletarDono(Guid id)
        {
            try
            {
                                       
                Dono? dono = _repository.FindById(id);
                if (dono == null)
                {
                    throw new Exception("Dono não encontrado");
                }

                                                    
                                                                            
                dono.RemovedAt = DateTime.Now;
                
                                    
                _repository.Update(dono);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

                     
                           
                                                               
            
                  
                                                      
                                           
                                             
                      
        public Dono? GetById(Guid id)
        {
            try
            {
                                 
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
