using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Workflow.Services.LeaveRequestWorkflow.WorkflowStep
{
    public class NotifyStep : StepBody
    {
        public string Message { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("Notification: " + Message);
            return ExecutionResult.Next();
        }
    }

}
