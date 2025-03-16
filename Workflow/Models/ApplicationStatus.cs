
namespace Workflow.Models
{
    public enum ApplicationStatus
    {
        Pending = 1,
        InProcess = 2,
        Forwarded = 3,
        Approved = 4,
        Rejected = 5,
        OnHold = 6,
        Completed = 7,
        Canceled = 8
    }
}