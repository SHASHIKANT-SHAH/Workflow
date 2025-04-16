using Microsoft.AspNetCore.Mvc;
using WorkflowCore.Interface;
using Workflow.Models;
using Workflow.Services.LeaveRequestWorkflow.model;
using WorkflowCore.Persistence.EntityFramework.Services;
using Workflow.Data;

namespace Workflow.Controllers
{
    public class WorkflowController : Controller
    {
        private readonly IWorkflowHost _host;
        private readonly ApplicationDbContext _context;

        public WorkflowController(IWorkflowHost host, ApplicationDbContext context)
        {
            _host = host;
            _context = context;
        }

        public IActionResult Index()
        {
            var requests = _context.LeaveRequests.ToList();
            return View(requests);
        }

        [HttpPost]
        public async Task<IActionResult> StartWorkflow(string employeeName)
        {
            var workflowId = await _host.StartWorkflow("LeaveApprovalWorkflow", 1, new LeaveRequestData { EmployeeName = employeeName });

            var req = new LeaveRequest
            {
                EmployeeName = employeeName,
                WorkflowId = workflowId
            };

            _context.LeaveRequests.Add(req);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ManagerApprove(int id, string decision)
        {
            var request = _context.LeaveRequests.Find(id);
            if (request == null) return NotFound();

            _host.PublishEvent("ManagerApproval", request.EmployeeName, decision);
            request.ManagerDecision = decision;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult HrApprove(int id, string decision)
        {
            var request = _context.LeaveRequests.Find(id);
            if (request == null) return NotFound();

            _host.PublishEvent("HrApproval", request.EmployeeName, decision);
            request.HrDecision = decision;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }

}
