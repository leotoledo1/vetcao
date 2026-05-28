using VetClinic.Entities;

namespace VetClinic.Interfaces.IRepositories
{
    // Contrato do repositorio de donos.
    public interface IDonoRepository
    {
        public List<Dono> GetAll();
        public Dono? FindById(Guid id);
        public bool CpfJaExiste(string cpf);
        public void Create(Dono dono);
        public void Update(Dono dono);
        public void Delete(Dono dono);
    }
}
