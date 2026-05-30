using TaskManager.Core.Enums;

namespace TaskManager.Core.Models.Task
{
    public class UpdateTaskModel
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ResponsibleUserEmail { get; set; }
        public Guid? CategoryId { get; set; }
        public TaskStatusEnum? Status { get; set; }
        public DateOnly? Term { get; set; }
    }
}