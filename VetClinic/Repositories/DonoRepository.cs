using VetClinic.Data;
using VetClinic.Entities;
using VetClinic.Interfaces.IRepositories;

namespace VetClinic.Repositories
{
    public class DonoRepository : IDonoRepository
    {
        public readonly Context _database;

        public DonoRepository(Context context)
        {
            _database = context;
        }

        public void Create(Dono dono)
        {
            _database.Donos.Add(dono);
            _database.SaveChanges();
        }

        public void Delete(Dono dono)
        {
            _database.Donos.Remove(dono);
            _database.SaveChanges();
        }

        public List<Dono> GetAll()
        {
            return _database.Donos
                .Where(d => d.RemovedAt == null)
                .OrderBy(d => d.Nome)
                .ToList();
        }

        public Dono? FindById(Guid id)
        {
            return _database.Donos
                .Where(d => d.Id == id && d.RemovedAt == null)
                .FirstOrDefault();
        }

        public bool CpfJaExiste(string cpf)
        {
            return _database.Donos
                .Any(d => d.Cpf.Value == cpf && d.RemovedAt == null);
        }

        public void Update(Dono dono)
        {
            _database.Donos.Update(dono);
            _database.SaveChanges();
        }
    }
}
