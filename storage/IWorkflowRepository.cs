using WorkflowEngine.Models;

namespace WorkflowEngine.Storage
{
    public interface IWorkflowRepository
    {
        // Workflow Definitions
        Task<WorkflowDefinition> SaveDefinitionAsync(WorkflowDefinition definition);
        Task<WorkflowDefinition?> GetDefinitionAsync(string id);
        Task<List<WorkflowDefinition>> GetAllDefinitionsAsync();
        Task<bool> DeleteDefinitionAsync(string id);

        // Workflow Instances
        Task<WorkflowInstance> SaveInstanceAsync(WorkflowInstance instance);
        Task<WorkflowInstance?> GetInstanceAsync(string id);
        Task<List<WorkflowInstance>> GetAllInstancesAsync();
        Task<List<WorkflowInstance>> GetInstancesByDefinitionIdAsync(string definitionId);
        Task<bool> DeleteInstanceAsync(string id);
    }
}
