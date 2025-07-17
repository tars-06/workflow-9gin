using WorkflowEngine.Models;
using WorkflowEngine.DTOs;
using WorkflowEngine.Storage;
using WorkflowEngine.Validation;
using WorkflowEngine.Exceptions;

namespace WorkflowEngine.Services
{
    public class WorkflowService : IWorkflowService
    {
        private readonly IWorkflowRepository _repository;
        private readonly WorkflowValidator _validator;

        public WorkflowService(IWorkflowRepository repository, WorkflowValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        // Workflow Definition Operations
        public async Task<WorkflowDefinition> CreateDefinitionAsync(CreateWorkflowDefinitionDto dto)
        {
            var definition = new WorkflowDefinition
            {
                Id = Guid.NewGuid().ToString(),
                Name = dto.Name,
                Description = dto.Description,
                States = dto.States,
                Actions = dto.Actions,
                CreatedAt = DateTime.UtcNow
            };

            _validator.ValidateDefinition(definition);
            return await _repository.SaveDefinitionAsync(definition);
        }

        public async Task<WorkflowDefinition> GetDefinitionAsync(string id)
        {
            var definition = await _repository.GetDefinitionAsync(id);
            if (definition == null)
                throw new WorkflowNotFoundException($"Workflow definition with ID '{id}' not found");

            return definition;
        }

        public async Task<List<WorkflowDefinition>> GetAllDefinitionsAsync()
        {
            return await _repository.GetAllDefinitionsAsync();
        }

        public async Task<bool> DeleteDefinitionAsync(string id)
        {
            var definition = await _repository.GetDefinitionAsync(id);
            if (definition == null)
                throw new WorkflowNotFoundException($"Workflow definition with ID '{id}' not found");

            // Check if there are active instances
            var instances = await _repository.GetInstancesByDefinitionIdAsync(id);
            if (instances.Any(i => !i.IsCompleted))
                throw new WorkflowValidationException("Cannot delete workflow definition with active instances");

            return await _repository.DeleteDefinitionAsync(id);
        }

        // Workflow Instance Operations
        public async Task<WorkflowInstance> StartInstanceAsync(StartWorkflowInstanceDto dto)
        {
            var definition = await GetDefinitionAsync(dto.WorkflowDefinitionId);
            var initialState = definition.States.First(s => s.IsInitial);

            var instance = new WorkflowInstance
            {
                Id = Guid.NewGuid().ToString(),
                WorkflowDefinitionId = dto.WorkflowDefinitionId,
                CurrentStateId = initialState.Id,
                CreatedAt = DateTime.UtcNow,
                IsCompleted = initialState.IsFinal,
                History = new List<HistoryEntry>()
            };

            return await _repository.SaveInstanceAsync(instance);
        }

        public async Task<WorkflowInstance> GetInstanceAsync(string id)
        {
            var instance = await _repository.GetInstanceAsync(id);
            if (instance == null)
                throw new WorkflowNotFoundException($"Workflow instance with ID '{id}' not found");

            return instance;
        }

        public async Task<List<WorkflowInstance>> GetAllInstancesAsync()
        {
            return await _repository.GetAllInstancesAsync();
        }

        public async Task<WorkflowInstance> ExecuteActionAsync(string instanceId, ExecuteActionDto dto)
        {
            var instance = await GetInstanceAsync(instanceId);
            var definition = await GetDefinitionAsync(instance.WorkflowDefinitionId);

            // Validate the action execution
            _validator.ValidateActionExecution(instance, definition, dto.ActionId);

            var action = definition.Actions.First(a => a.Id == dto.ActionId);
            var targetState = definition.States.First(s => s.Id == action.ToState);

            // Create history entry
            var historyEntry = new HistoryEntry
            {
                ActionId = dto.ActionId,
                ActionName = action.Name,
                FromStateId = instance.CurrentStateId,
                ToStateId = action.ToState,
                Timestamp = DateTime.UtcNow
            };

            // Update instance
            instance.CurrentStateId = action.ToState;
            instance.History.Add(historyEntry);
            instance.IsCompleted = targetState.IsFinal;

            return await _repository.SaveInstanceAsync(instance);
        }

        public async Task<List<WorkflowInstance>> GetInstancesByDefinitionIdAsync(string definitionId)
        {
            return await _repository.GetInstancesByDefinitionIdAsync(definitionId);
        }

        // Utility Operations
        public async Task<List<Action>> GetAvailableActionsAsync(string instanceId)
        {
            var instance = await GetInstanceAsync(instanceId);
            var definition = await GetDefinitionAsync(instance.WorkflowDefinitionId);

            if (instance.IsCompleted)
                return new List<Action>();

            return definition.Actions
                .Where(a => a.Enabled && a.FromStates.Contains(instance.CurrentStateId))
                .ToList();
        }

        public async Task<State> GetCurrentStateAsync(string instanceId)
        {
            var instance = await GetInstanceAsync(instanceId);
            var definition = await GetDefinitionAsync(instance.WorkflowDefinitionId);

            return definition.States.First(s => s.Id == instance.CurrentStateId);
        }
    }
}
