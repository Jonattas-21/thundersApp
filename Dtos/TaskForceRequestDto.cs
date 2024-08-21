namespace thundersApp.Dtos
{
    public class TaskForceRequestDto
    {
        public required string Name { get; set; }
        public int Priority { get; set; }
        public required string Description { get; set; }
        public required string Assignee { get; set; }
        public required Guid OriginId { get; set; }
    }
}
