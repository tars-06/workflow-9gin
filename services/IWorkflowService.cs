using WorkflowEngine.Models;
using WorkflowEngine.DTOs;

namespace WorkflowEngine.Services
{
    public interface IWorkflowService
    {
        // Workflow Definition Operations
        Task<WorkflowDefinition> CreateDefinitionAsync(CreateWorkflowDefinitionDto dto);
        Task<WorkflowDefinition> GetDefinitionAsync(string id);
        Task<List<WorkflowDefinition>> GetAllDefinitionsAsync();
        Task<bool> DeleteDefinitionAsync(string id);

        // Workflow Instance Operations
        Task<WorkflowInstance> StartInstanceAsync(StartWorkflowInstanceDto dto);
        Task<WorkflowInstance> GetInstanceAsync(string id);
        Task<List<WorkflowInstance>> GetAllInstancesAsync();
        Task<WorkflowInstance> ExecuteActionAsync(string instanceId, ExecuteActionDto dto);
        Task<List<WorkflowInstance>> GetInstancesByDefinitionIdAsync(string definitionId);

        // Utility Operations
        Task<List<Action>> GetAvailableActionsAsync(string instanceId);
        Task<State> GetCurrentStateAsync(string instanceId);
    }
}
