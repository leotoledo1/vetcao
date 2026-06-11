using VetClinic.Data;
using VetClinic.Entities;
using VetClinic.Interfaces.IRepositories;

namespace VetClinic.Repositories
{
                                                              
    public class VeterinarioRepository : IVeterinarioRepository
    {
        public readonly Context _database;

        public VeterinarioRepository(Context context)
        {
            _database = context;
        }

        public void Create(Veterinario veterinario)
        {
            _database.Veterinarios.Add(veterinario);
            _database.SaveChanges();
        }

        public List<Veterinario> GetAll()
        {
            return _database.Veterinarios
                .Where(v => v.RemovedAt == null)
                .OrderBy(v => v.Nome)
                .ToList();
        }

        public Veterinario? FindById(Guid id)
        {
            return _database.Veterinarios
                .Where(v => v.Id == id && v.RemovedAt == null)
                .FirstOrDefault();
        }
    }
}
