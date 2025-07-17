using WorkflowEngine.Models;

namespace WorkflowEngine.DTOs
{
    public class CreateWorkflowDefinitionDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<State> States { get; set; } = new();
        public List<Models.Action> Actions { get; set; } = new(); 
    }
}
