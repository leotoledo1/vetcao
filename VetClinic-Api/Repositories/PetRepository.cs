using Microsoft.EntityFrameworkCore;
using VetClinic.Data;
using VetClinic.Entities;
using VetClinic.Interfaces.IRepositories;

namespace VetClinic.Repositories
{
                                               
    public class PetRepository : IPetRepository
    {
        public readonly Context _database;

        public PetRepository(Context context)
        {
            _database = context;
        }

        public void Create(Pet pet)
        {
            _database.Pets.Add(pet);
            _database.SaveChanges();
        }

        public void Delete(Pet pet)
        {
            _database.Pets.Remove(pet);
            _database.SaveChanges();
        }

        public List<Pet> GetAll()
        {
            return _database.Pets
                .Include(p => p.Dono)
                .Where(p => p.RemovedAt == null)
                .OrderBy(p => p.Nome)
                .ToList();
        }

        public Pet? FindById(Guid id)
        {
            return _database.Pets
                .Include(p => p.Dono)
                .Where(p => p.Id == id && p.RemovedAt == null)
                .FirstOrDefault();
        }

        public bool TemConsultaFutura(Guid petId)
        {
            return _database.Consultas
                .Any(c => c.PetId == petId
                       && c.Status == "Agendada"
                       && c.DataHora > DateTime.Now);
        }

        public void Update(Pet pet)
        {
            _database.Pets.Update(pet);
            _database.SaveChanges();
        }
    }
}
