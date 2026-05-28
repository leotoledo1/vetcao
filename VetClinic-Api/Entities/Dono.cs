using VetClinic.ValueObjects;

namespace VetClinic.Entities
{
    // Entidade principal que representa o dono cadastrado na clinica.
    /// ========================================
    /// ENTITY: Dono
    /// RESPONSABILIDADE: Representa um DONO de PET no sistema
    /// 
    /// Uma entidade é uma classe que será uma tabela no banco de dados
    /// Cada propriedade vira uma coluna
    /// ========================================
    public class Dono : EntityBase
    {
        // Propriedades do Dono
        public string Nome { get; set; } = string.Empty;        // Nome do dono
        public Cpf Cpf { get; set; } = null!;                   // CPF - ValueObject que valida
        public string Telefone { get; set; } = string.Empty;    // Telefone de contato
        public string Email { get; set; } = string.Empty;       // Email de contato

        // Relacionamento: um Dono pode ter VÁRIOS Pets
        private readonly List<Pet> _pets = new List<Pet>();
        
        // Propriedade somente leitura para acessar os pets
        // IReadOnlyCollection protege contra modificações diretas
        public IReadOnlyCollection<Pet> Pets => _pets.AsReadOnly();

        public Dono() { }

        /// <summary>
        /// MÉTODO: AdicionarPet
        /// RESPONSABILIDADE: Adicionar um pet a este dono
        /// 
        /// Validação: verifica se o pet é nulo antes de adicionar
        /// </summary>
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
