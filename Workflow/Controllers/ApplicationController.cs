using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Workflow.Data;
using Workflow.Models;
using Workflow.Repositories;
using Workflow.Services;

namespace Workflow.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly IApplicationRepository _repository;
        private readonly ApplicationDbContext _context;

        public ApplicationController(IApplicationRepository repository, ApplicationDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        // Show all applications
        public IActionResult Index()
        {
            var data = _repository.GetAllApplications();
            return View(data);
        }

        // Show create form
        public IActionResult Create()
        {
            return View(new Application());
        }

        [HttpPost]
        public IActionResult Create(Application application)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(application);
            //}

            _repository.AddApplication(application);
            return RedirectToAction("Index");
        }

        // Show status edit form
        public IActionResult EditStatus(int id)
        {
            var application = _repository.GetApplicationById(id);
            if (application == null) return NotFound();

            // Get valid status transitions using StatusId (int)
            if (!ApplicationWorkflow.ValidTransitions.TryGetValue(application.StatusId, out var validStatusIds))
            {
                validStatusIds = new List<int>(); // Default to empty list if no transitions are found
            }

            // Fetch valid statuses from DB based on validStatusIds
            var validStatuses = _context.ApplicationStatuses
                .Where(s => validStatusIds.Contains(s.Id))
                .ToList();

            ViewBag.ValidStatuses = new SelectList(validStatuses, "Id", "Name");

            return View(application);
        }


        [HttpPost]
        public IActionResult EditStatus(int id, int newStatusId, string remarks)
        {
            var application = _repository.GetApplicationById(id);
            if (application == null) return NotFound();

            // Check if transition is allowed
            if (!ApplicationWorkflow.CanTransition(application.StatusId, newStatusId))
            {
                ModelState.AddModelError("Status", "Invalid status transition.");
                return View(application);
            }

            // Update status
            var success = _repository.UpdateApplicationStatus(id, newStatusId, "Admin", remarks);
            if (!success)
            {
                ModelState.AddModelError("Status", "Failed to update status.");
                return View(application);
            }

            return RedirectToAction("Index");
        }
    }
}
