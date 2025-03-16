using Microsoft.AspNetCore.Mvc;
using Workflow.Data;
using Workflow.Repositories;
using WorkflowCore.Interface;

namespace Workflow.Controllers
{
    public class ManualWorkflowApplicationController : Controller
    {
        private readonly IApplicationRepository _repository;
        private readonly IWorkflowHost _workflowHost;
        private readonly ApplicationDbContext _context;

        public ManualWorkflowApplicationController(IApplicationRepository repository, IWorkflowHost workflowHost, ApplicationDbContext context)
        {
            _repository = repository;
            _workflowHost = workflowHost;
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_repository.GetAllApplications());
        }

        public IActionResult EditStatus(int id)
        {
            var application = _repository.GetApplicationById(id);
            if (application == null) return NotFound();

            // Get valid status transitions
            var validStatuses = Workflow.Services.ApplicationWorkflow.ValidTransitions
                .FirstOrDefault(x => x.Key == application.StatusId)
                .Value ?? new List<int>();

            ViewBag.ValidStatuses = _context.ApplicationStatuses.Where(s => validStatuses.Contains(s.Id)).ToList();
            return View(application);
        }

        [HttpPost]
        public IActionResult EditStatus(int id, int newStatusId, string remarks)
        {
            try
            {
                _repository.UpdateApplicationStatus(id, newStatusId, "Admin", remarks);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(_repository.GetApplicationById(id));
            }
        }

        public IActionResult StartWorkflow(int applicationId)
        {
            _workflowHost.StartWorkflow("ApplicationWorkflow", _repository.GetApplicationById(applicationId));
            return RedirectToAction("Index");
        }
    }


}
