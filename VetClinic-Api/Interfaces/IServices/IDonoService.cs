using VetClinic.DTOs;
using VetClinic.Entities;

namespace VetClinic.Interfaces.IServices
{
                                    
    public interface IDonoService
    {
        public List<Dono> ListarDonos();
        public Dono? GetById(Guid id);
        public void CriarDono(CreateDonoDTO dto);
        public void DeletarDono(Guid id);
    }
}
