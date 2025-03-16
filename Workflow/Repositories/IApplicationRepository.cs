using Workflow.Models;

namespace Workflow.Repositories
{
    public interface IApplicationRepository
    {
        List<Application> GetAllApplications();
        Application GetApplicationById(int id);
        void AddApplication(Application application);
        void UpdateApplication(Application application);
        bool UpdateApplicationStatus(int applicationId, int newStatusId, string changedBy, string remarks);
    }

}
