using VetClinic.Entities;

namespace VetClinic.Interfaces.IRepositories
{
    public interface IVeterinarioRepository
    {
        public List<Veterinario> GetAll();
        public Veterinario? FindById(Guid id);
        public void Create(Veterinario veterinario);
    }
}
