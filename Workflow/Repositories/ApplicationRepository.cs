using Workflow.Data;
using Workflow.Models;
using Workflow.Services;
using Microsoft.EntityFrameworkCore;

namespace Workflow.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all applications with Status and History
        public List<Application> GetAllApplications() =>
            _context.Applications
                .Include(a => a.Status) // Include Status Table
                .Include(a => a.ApplicationHistories)
                    .ThenInclude(h => h.PreviousStatus)
                .Include(a => a.ApplicationHistories)
                    .ThenInclude(h => h.NewStatus)
                .ToList();

        // Get a single application with full details
        public Application GetApplicationById(int id)
        {
            return _context.Applications
                .AsNoTracking() //  Forces fresh data from DB
                .Include(a => a.Status)
                .Include(a => a.ApplicationHistories)
                    .ThenInclude(h => h.PreviousStatus)
                .Include(a => a.ApplicationHistories)
                    .ThenInclude(h => h.NewStatus)
                .FirstOrDefault(a => a.Id == id)!;
        }

        // Add a new application (set default status to Pending)
        public void AddApplication(Application application)
        {
            var pendingStatus = _context.ApplicationStatuses
                .FirstOrDefault(s => s.Name == "Pending"); // Fetch Pending Status

            if (pendingStatus != null)
            {
                application.StatusId = pendingStatus.Id; // Assign Status Foreign Key
                _context.Applications.Add(application);
                _context.SaveChanges();
            }
        }

        // Update application (modify status correctly)
        public void UpdateApplication(Application application)
        {
            _context.Applications.Update(application);
            _context.SaveChanges();
        }

        // Update application status with history
        public bool UpdateApplicationStatus(int applicationId, int newStatusId, string changedBy, string remarks)
        {
            var application = _context.Applications
                .Include(a => a.Status)
                .FirstOrDefault(a => a.Id == applicationId);

            if (application == null) return false;

            var newStatus = _context.ApplicationStatuses.FirstOrDefault(s => s.Id == newStatusId);
            if (newStatus == null) return false;

            // Validate transition (using ApplicationWorkflow static class)
            if (!ApplicationWorkflow.CanTransition(application.StatusId, newStatusId))
                return false;

            var history = new ApplicationHistory
            {
                ApplicationId = applicationId,
                PreviousStatusId = application.StatusId, // Old Status
                NewStatusId = newStatus.Id, // New Status
                ChangedBy = changedBy,
                Remarks = remarks,
                ChangedAt = DateTime.Now
            };

            // Update Status and Save History
            application.StatusId = newStatus.Id;
            _context.ApplicationHistories.Add(history);
            _context.SaveChanges();
            return true;
        }
    }

}
