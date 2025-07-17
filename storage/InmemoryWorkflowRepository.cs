using System.Collections.Concurrent;
using WorkflowEngine.Models;

namespace WorkflowEngine.Storage
{
    public class InMemoryWorkflowRepository : IWorkflowRepository
    {
        private readonly ConcurrentDictionary<string, WorkflowDefinition> _definitions = new();
        private readonly ConcurrentDictionary<string, WorkflowInstance> _instances = new();

        // Workflow Definitions
        public Task<WorkflowDefinition> SaveDefinitionAsync(WorkflowDefinition definition)
        {
            _definitions.AddOrUpdate(definition.Id, definition, (key, oldValue) => definition);
            return Task.FromResult(definition);
        }

        public Task<WorkflowDefinition?> GetDefinitionAsync(string id)
        {
            _definitions.TryGetValue(id, out var definition);
            return Task.FromResult(definition);
        }

        public Task<List<WorkflowDefinition>> GetAllDefinitionsAsync()
        {
            return Task.FromResult(_definitions.Values.ToList());
        }

        public Task<bool> DeleteDefinitionAsync(string id)
        {
            return Task.FromResult(_definitions.TryRemove(id, out _));
        }

        // Workflow Instances
        public Task<WorkflowInstance> SaveInstanceAsync(WorkflowInstance instance)
        {
            _instances.AddOrUpdate(instance.Id, instance, (key, oldValue) => instance);
            return Task.FromResult(instance);
        }

        public Task<WorkflowInstance?> GetInstanceAsync(string id)
        {
            _instances.TryGetValue(id, out var instance);
            return Task.FromResult(instance);
        }

        public Task<List<WorkflowInstance>> GetAllInstancesAsync()
        {
            return Task.FromResult(_instances.Values.ToList());
        }

        public Task<List<WorkflowInstance>> GetInstancesByDefinitionIdAsync(string definitionId)
        {
            var instances = _instances.Values
                .Where(i => i.WorkflowDefinitionId == definitionId)
                .ToList();
            return Task.FromResult(instances);
        }

        public Task<bool> DeleteInstanceAsync(string id)
        {
            return Task.FromResult(_instances.TryRemove(id, out _));
        }
    }
}
