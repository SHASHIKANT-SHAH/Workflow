using Workflow.Data;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Workflow.Services.LeaveRequestWorkflow.WorkflowStep
{
    public class UpdateWorkflowStatusStep : StepBody
    {
        private readonly ApplicationDbContext _context;

        public UpdateWorkflowStatusStep(ApplicationDbContext context)
        {
            _context = context;
        }

        public Guid WorkflowInstanceId { get; set; }
        public string NewStatus { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            var instance = _context.WorkflowInstanceInfos
                                   .FirstOrDefault(w => w.Id == WorkflowInstanceId);
            if (instance != null)
            {
                instance.Status = NewStatus;
                instance.EndDate = DateTime.UtcNow;
                _context.SaveChanges();
            }

            return ExecutionResult.Next();
        }
    }

}
