using Workflow.Models;

namespace Workflow.Services
{
    public static class ApplicationWorkflow
    {
        //    public static readonly Dictionary<ApplicationStatus, List<ApplicationStatus>> ValidTransitions = new()
        //{
        //    { ApplicationStatus.Pending, new List<ApplicationStatus> { ApplicationStatus.InProcess, ApplicationStatus.Rejected } },
        //    { ApplicationStatus.InProcess, new List<ApplicationStatus> { ApplicationStatus.Forwarded, ApplicationStatus.OnHold, ApplicationStatus.Rejected } },
        //    { ApplicationStatus.Forwarded, new List<ApplicationStatus> { ApplicationStatus.Approved, ApplicationStatus.Rejected } },
        //    { ApplicationStatus.Approved, new List<ApplicationStatus> { ApplicationStatus.Completed } },
        //    { ApplicationStatus.Rejected, new List<ApplicationStatus> { ApplicationStatus.Canceled } },
        //    { ApplicationStatus.OnHold, new List<ApplicationStatus> { ApplicationStatus.InProcess, ApplicationStatus.Canceled } },
        //    { ApplicationStatus.Completed, new List<ApplicationStatus>() },
        //    { ApplicationStatus.Canceled, new List<ApplicationStatus>() }
        //};

        //    public static bool CanTransition(ApplicationStatus current, ApplicationStatus next)
        //    {
        //        return ValidTransitions.ContainsKey(current) && ValidTransitions[current].Contains(next);
        //    }

        public static readonly Dictionary<int, List<int>> ValidTransitions = new()
        {
            { 1, new List<int> { 2, 5 } }, // Pending → InProcess, Rejected
            { 2, new List<int> { 3, 6, 5 } }, // InProcess → Forwarded, OnHold, Rejected
            { 3, new List<int> { 4, 5 } }, // Forwarded → Approved, Rejected
            { 4, new List<int> { 7 } }, // Approved → Completed
            { 5, new List<int> { 8 } }, // Rejected → Canceled
            { 6, new List<int> { 2, 8 } }, // OnHold → InProcess, Canceled
            { 7, new List<int>() }, // Completed → (No transitions)
            { 8, new List<int>() } // Canceled → (No transitions)
        };

        public static bool CanTransition(int currentStatusId, int newStatusId)
        {
            return ValidTransitions.ContainsKey(currentStatusId) &&
                   ValidTransitions[currentStatusId].Contains(newStatusId);
        }
    }

}
