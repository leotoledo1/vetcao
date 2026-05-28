using VetClinic.Entities;

namespace VetClinic.Interfaces.IRepositories
{
    // Contrato do repositorio de pets.
    public interface IPetRepository
    {
        public List<Pet> GetAll();
        public Pet? FindById(Guid id);
        public void Create(Pet pet);
        public void Update(Pet pet);
        public void Delete(Pet pet);
        public bool TemConsultaFutura(Guid petId);
    }
}
