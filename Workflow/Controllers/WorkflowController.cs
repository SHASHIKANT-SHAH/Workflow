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
            var data = new LeaveRequestData
            {
                EmployeeName = employeeName,
                ManagerDecision = "Pending",
                HrDecision = "Pending",
                WorkflowInstanceId = Guid.NewGuid() // create ID beforehand
            };

            // Start workflow and get workflowId
            var workflowId = await _host.StartWorkflow("LeaveApprovalWorkflow", 1, data);

            //var req = new LeaveRequest
            //{
            //    EmployeeName = employeeName,
            //    WorkflowInstanceInfoId = Guid.Parse(workflowId),
            //    ManagerDecisionId = 1, // Assuming 1 = Pending
            //    HrDecisionId = 1
            //};

            //// Add leave request and save
            //_context.LeaveRequests.Add(req);
            //await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ManagerApprove(int id, string decision)
        {
            var request = await _context.LeaveRequests
                            .Include(r => r.WorkflowInstanceInfo)
                            .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null) return NotFound();

            var status = await _context.Statuses.FirstOrDefaultAsync(s => s.Name == decision);
            if (status == null) return BadRequest("Invalid decision");

            request.ManagerDecisionId = status.Id;

            // Update the decision and workflow
            await _context.SaveChangesAsync();
            await _host.PublishEvent("ManagerApproval", request.EmployeeName, decision);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> HrApprove(int id, string decision)
        {
            var request = await _context.LeaveRequests
                            .Include(r => r.WorkflowInstanceInfo)
                            .Include(r => r.ManagerDecision)
                            .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null) return NotFound();

            var status = await _context.Statuses.FirstOrDefaultAsync(s => s.Name == decision);
            if (status == null) return BadRequest("Invalid decision");

            request.HrDecisionId = status.Id;

            // Update the decision and workflow
            await _context.SaveChangesAsync();
            await _host.PublishEvent("HrApproval", request.EmployeeName, decision);

            // Workflow status update handled by workflow itself (inside UpdateWorkflowStatusStep)

            return RedirectToAction("Index");
        }
    }
}
