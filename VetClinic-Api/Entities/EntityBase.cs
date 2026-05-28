namespace VetClinic.Entities
{
    // Base comum para todas as entidades do sistema.
    public class EntityBase
    {
        public Guid Id { get; set; }
        public DateTime? RemovedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public EntityBase()
        {
            Id = Guid.NewGuid();
        }
    }
}
