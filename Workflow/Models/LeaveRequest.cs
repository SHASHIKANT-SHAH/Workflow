namespace Workflow.Models
{
    public class LeaveRequest
    {
        public int Id { get; set; }
        public string? EmployeeName { get; set; }
        public string? ManagerDecision { get; set; }
        public string? HrDecision { get; set; }
        public string? WorkflowId { get; set; }
    }
}
