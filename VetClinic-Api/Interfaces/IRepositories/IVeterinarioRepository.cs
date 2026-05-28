using VetClinic.Entities;

namespace VetClinic.Interfaces.IRepositories
{
    // Contrato do repositorio de veterinarios.
    public interface IVeterinarioRepository
    {
        public List<Veterinario> GetAll();
        public Veterinario? FindById(Guid id);
        public void Create(Veterinario veterinario);
    }
}
