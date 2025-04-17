namespace Workflow.Models
{
    public class LeaveRequestViewModel
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string ManagerStatus { get; set; }
        public string HrStatus { get; set; }
        public string WorkflowId { get; set; }
    }

}
