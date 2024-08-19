namespace Domain.Entities
{
    public class Grape : BaseEntity
    {
        public required string Name { get; set; }
        public required string Origin { get; set; }
        public required string Description { get; set; }
    }
}
