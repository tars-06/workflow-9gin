namespace WorkflowEngine.Models
{
    public class HistoryEntry
    {
        public string ActionId { get; set; } = string.Empty;
        public string ActionName { get; set; } = string.Empty;
        public string FromStateId { get; set; } = string.Empty;
        public string ToStateId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
