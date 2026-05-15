using VetClinic.ValueObjects;

namespace VetClinic.Entities
{
    public class Dono : EntityBase
    {
        public string Nome { get; set; } = string.Empty;
        public Cpf Cpf { get; set; } = null!;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        private readonly List<Pet> _pets = new List<Pet>();
        public IReadOnlyCollection<Pet> Pets => _pets.AsReadOnly();

        public Dono() { }

        public void AdicionarPet(Pet pet)
        {
            if (pet == null)
            {
                throw new Exception("Pet inválido");
            }

            _pets.Add(pet);
        }
    }
}
