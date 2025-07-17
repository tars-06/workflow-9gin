namespace WorkflowEngine.Exceptions
{
    public class WorkflowException : Exception
    {
        public WorkflowException(string message) : base(message) { }
        public WorkflowException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class WorkflowValidationException : WorkflowException
    {
        public WorkflowValidationException(string message) : base(message) { }
    }

    public class WorkflowNotFoundException : WorkflowException
    {
        public WorkflowNotFoundException(string message) : base(message) { }
    }

    public class InvalidStateTransitionException : WorkflowException
    {
        public InvalidStateTransitionException(string message) : base(message) { }
    }
}
