using WorkflowEngine.Models;
using WorkflowEngine.Exceptions;

namespace WorkflowEngine.Validation
{
    public class WorkflowValidator
    {
        public void ValidateDefinition(WorkflowDefinition definition)
        {
            if (string.IsNullOrWhiteSpace(definition.Name))
                throw new WorkflowValidationException("Workflow definition name is required");

            if (!definition.States.Any())
                throw new WorkflowValidationException("Workflow definition must have at least one state");

            ValidateStates(definition.States);
            ValidateActions(definition.Actions, definition.States);
        }

        private void ValidateStates(List<State> states)
        {
            // Check for duplicate state IDs
            var duplicateStateIds = states.GroupBy(s => s.Id)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key);

            if (duplicateStateIds.Any())
                throw new WorkflowValidationException($"Duplicate state IDs found: {string.Join(", ", duplicateStateIds)}");

            // Check for exactly one initial state
            var initialStates = states.Where(s => s.IsInitial).ToList();
            if (initialStates.Count == 0)
                throw new WorkflowValidationException("Workflow must have exactly one initial state");
            if (initialStates.Count > 1)
                throw new WorkflowValidationException("Workflow cannot have multiple initial states");

            // Validate individual states
            foreach (var state in states)
            {
                if (string.IsNullOrWhiteSpace(state.Id))
                    throw new WorkflowValidationException("State ID is required");
                if (string.IsNullOrWhiteSpace(state.Name))
                    throw new WorkflowValidationException($"State name is required for state {state.Id}");
            }
        }

        private void ValidateActions(List<Action> actions, List<State> states)
        {
            var stateIds = states.Select(s => s.Id).ToHashSet();

            // Check for duplicate action IDs
            var duplicateActionIds = actions.GroupBy(a => a.Id)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key);

            if (duplicateActionIds.Any())
                throw new WorkflowValidationException($"Duplicate action IDs found: {string.Join(", ", duplicateActionIds)}");

            // Validate individual actions
            foreach (var action in actions)
            {
                if (string.IsNullOrWhiteSpace(action.Id))
                    throw new WorkflowValidationException("Action ID is required");
                if (string.IsNullOrWhiteSpace(action.Name))
                    throw new WorkflowValidationException($"Action name is required for action {action.Id}");

                // Validate fromStates
                if (!action.FromStates.Any())
                    throw new WorkflowValidationException($"Action {action.Id} must have at least one source state");

                var invalidFromStates = action.FromStates.Where(s => !stateIds.Contains(s));
                if (invalidFromStates.Any())
                    throw new WorkflowValidationException($"Action {action.Id} references invalid source states: {string.Join(", ", invalidFromStates)}");

                // Validate toState
                if (string.IsNullOrWhiteSpace(action.ToState))
                    throw new WorkflowValidationException($"Action {action.Id} must have a target state");
                if (!stateIds.Contains(action.ToState))
                    throw new WorkflowValidationException($"Action {action.Id} references invalid target state: {action.ToState}");
            }
        }

        public void ValidateActionExecution(WorkflowInstance instance, WorkflowDefinition definition, string actionId)
        {
            var action = definition.Actions.FirstOrDefault(a => a.Id == actionId);
            if (action == null)
                throw new WorkflowValidationException($"Action {actionId} not found in workflow definition");

            if (!action.Enabled)
                throw new InvalidStateTransitionException($"Action {actionId} is disabled");

            var currentState = definition.States.FirstOrDefault(s => s.Id == instance.CurrentStateId);
            if (currentState == null)
                throw new WorkflowValidationException($"Current state {instance.CurrentStateId} not found in definition");

            if (currentState.IsFinal)
                throw new InvalidStateTransitionException("Cannot execute actions on final states");

            if (!action.FromStates.Contains(instance.CurrentStateId))
                throw new InvalidStateTransitionException($"Action {actionId} cannot be executed from current state {instance.CurrentStateId}");

            var targetState = definition.States.FirstOrDefault(s => s.Id == action.ToState);
            if (targetState == null)
                throw new WorkflowValidationException($"Target state {action.ToState} not found in definition");

            if (!targetState.Enabled)
                throw new InvalidStateTransitionException($"Target state {action.ToState} is disabled");
        }
    }
}
