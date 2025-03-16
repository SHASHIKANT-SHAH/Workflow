using Microsoft.AspNetCore.Mvc;
using WorkflowCore.Interface;
using Workflow.Models;

namespace Workflow.Controllers
{
    public class WorkflowController: Controller
    {
        private readonly IWorkflowHost _workflowHost;

        public WorkflowController(IWorkflowHost workflowHost)
        {
            _workflowHost = workflowHost;
        }

        [HttpGet]
        public async Task<IActionResult> StartWorkflow()
        {
            var workflowId = await _workflowHost.StartWorkflow("LoanApprovalWorkflow", new LoanApplicationData
            {
                ApplicantName = "John Doe",
                LoanAmount = 50000
            });

            return Ok(new { Message = "Workflow started", WorkflowId = workflowId });
        }
    }
}
