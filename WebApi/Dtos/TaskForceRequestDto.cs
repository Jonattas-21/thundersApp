namespace thundersApp.Dtos
{
    public class TaskForceRequestDto
    {
        public string? Name { get; set; }
        public int Priority { get; set; }
        public string? Description { get; set; }
        public string? Assignee { get; set; }
        public Guid OriginId { get; set; }
    }
}
