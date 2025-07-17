using WorkflowEngine.Models; // ADD this line if missing

namespace WorkflowEngine.Models
{
    public class WorkflowInstance
    {
        public string Id { get; set; } = string.Empty;
        public string WorkflowDefinitionId { get; set; } = string.Empty;
        public string CurrentStateId { get; set; } = string.Empty;
        public List<HistoryEntry> History { get; set; } = new();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsCompleted { get; set; }
    }
}
