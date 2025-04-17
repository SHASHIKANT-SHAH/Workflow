using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkflowCore.Interface;
using Workflow.Models;
using Workflow.Services.LeaveRequestWorkflow.model;
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
            var requests = _context.LeaveRequests
                            .Include(r => r.ManagerDecision)
                            .Include(r => r.HrDecision)
                            .Include(r => r.WorkflowInstanceInfo)
                            .ToList();

            return View(requests);
        }

        [HttpPost]
        public async Task<IActionResult> StartWorkflow(string employeeName)
        {
            // Create a new Workflow Instance
            var workflowInstance = new WorkflowInstanceInfo
            {
                StartDate = DateTime.UtcNow,
                Status = "InProgress"
            };

            _context.WorkflowInstanceInfos.Add(workflowInstance);
            await _context.SaveChangesAsync();  // Save WorkflowInstanceInfo to generate its ID

            // Now, create the LeaveRequest and associate it with the WorkflowInstanceInfo
            var req = new LeaveRequest
            {
                EmployeeName = employeeName,
                WorkflowInstanceInfoId = workflowInstance.Id,  // Link to WorkflowInstanceInfo
                ManagerDecisionId = 1, // Pending
                HrDecisionId = 1      // Pending
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

            var status = _context.Statuses.FirstOrDefault(s => s.Name == decision);
            if (status == null) return BadRequest("Invalid status");

            // Update Manager Decision on the LeaveRequest
            request.ManagerDecisionId = status.Id;
            _context.SaveChanges();

            // Now update the WorkflowInstanceInfo for this workflow
            var workflowInstance = _context.WorkflowInstanceInfos
                .FirstOrDefault(wi => wi.Id == request.WorkflowInstanceInfoId);

            if (workflowInstance != null)
            {
                // Update workflow status
                workflowInstance.Status = decision == "Approved" ? "InProgress" : "Manager Rejected";
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult HrApprove(int id, string decision)
        {
            var request = _context.LeaveRequests.Find(id);
            if (request == null) return NotFound();

            var status = _context.Statuses.FirstOrDefault(s => s.Name == decision);
            if (status == null) return BadRequest("Invalid status");

            // Update HR Decision on the LeaveRequest
            request.HrDecisionId = status.Id;
            _context.SaveChanges();

            // Now update the WorkflowInstanceInfo for this workflow
            var workflowInstance = _context.WorkflowInstanceInfos
                .FirstOrDefault(wi => wi.Id == request.WorkflowInstanceInfoId);

            if (workflowInstance != null)
            {
                // If both Manager and HR have made decisions, mark workflow as completed
                if (request.ManagerDecision != null && request.HrDecision != null)
                {
                    workflowInstance.Status = "Completed";  // Final status
                    workflowInstance.EndDate = DateTime.UtcNow;  // Mark end date
                    _context.SaveChanges();
                }
                else
                {
                    workflowInstance.Status = decision == "Approved" ? "InProgress" : "HR Rejected";
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

    }
}
