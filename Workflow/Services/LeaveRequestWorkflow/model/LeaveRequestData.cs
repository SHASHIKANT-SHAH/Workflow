namespace Workflow.Services.LeaveRequestWorkflow.model
{
    public class LeaveRequestData
    {
        public string EmployeeName { get; set; }
        public string ManagerDecision { get; set; }
        public string HrDecision { get; set; }

        public Guid WorkflowInstanceId { get; set; }
    }

}
