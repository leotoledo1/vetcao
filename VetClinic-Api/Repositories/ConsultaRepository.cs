using Microsoft.EntityFrameworkCore;
using VetClinic.Data;
using VetClinic.Entities;
using VetClinic.Interfaces.IRepositories;

namespace VetClinic.Repositories
{
                                                                       
                                                
                                      
                                                               
        
                           
                                         
                                                                  
                                                  
                                                                   
                                              
                                                
    public class ConsultaRepository : IConsultaRepository
    {
        public readonly Context _database;

        public ConsultaRepository(Context context)
        {
            _database = context;
        }

                     
                          
                                                                
                      
        public void Create(Consulta consulta)
        {
            _database.Consultas.Add(consulta);
            _database.SaveChanges();
        }

                     
                          
                                                                           
            
                       
                                                                
                                                                      
                                                                                    
            
                                                                               
                                                                     
                      
        public List<Consulta> GetAll()
        {
            return _database.Consultas
                                                          
                .Include(c => c.Pet)
                
                                                                  
                .Include(c => c.Veterinario)
                
                                                          
                .OrderBy(c => c.DataHora)
                
                                                                          
                .ToList();
        }

                     
                            
                                                                    
            
                                                                
                      
        public Consulta? FindById(Guid id)
        {
            return _database.Consultas
                .Include(c => c.Pet)
                .Include(c => c.Veterinario)
                .Where(c => c.Id == id)
                .FirstOrDefault();
        }

                     
                                                   
                                                                                  
            
                   
                                                                    
                                                                                    
                                                                              
            
                    
                                     
                                    
                                                          
            
                                       
                                                                                   
                                                                                  
                      
        public bool TemConflitoDeHorario(Guid veterinarioId, DateTime dataHora)
        {
                                                  
            DateTime inicioJanela = dataHora.AddMinutes(-30);
            
                                                   
            DateTime fimJanela = dataHora.AddMinutes(30);

                                                                   
            return _database.Consultas
                
                                                    
                .Any(c => c.VeterinarioId == veterinarioId
                
                                                                                                
                       && c.Status == "Agendada"
                       
                                                                       
                       && c.DataHora > inicioJanela
                       && c.DataHora < fimJanela);
        }

                     
                          
                                                              
            
                                                                
                          
                      
        public void Update(Consulta consulta)
        {
            _database.Consultas.Update(consulta);
            _database.SaveChanges();
        }
    }
}
