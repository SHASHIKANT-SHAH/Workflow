namespace Workflow.Models
{
    public class TableModel
    {
     

    }
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<LeaveRequest> ManagerLeaveRequests { get; set; }
        public ICollection<LeaveRequest> HrLeaveRequests { get; set; }
    }

    public class LeaveRequest
    {
        public int Id { get; set; }
        public string? EmployeeName { get; set; }
        public int? ManagerDecisionId { get; set; }
        public int? HrDecisionId { get; set; }
        public string? WorkflowInstanceInfoId { get; set; }

        public Status? ManagerDecision { get; set; }
        public Status? HrDecision { get; set; }
        public WorkflowInstanceInfo? WorkflowInstanceInfo { get; set; }
    }


    public class WorkflowInstanceInfo
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; } = "InProgress";
    }



}
